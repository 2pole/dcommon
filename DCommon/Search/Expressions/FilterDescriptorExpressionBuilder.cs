using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DCommon;

namespace DCommon.Search.Expressions
{
    internal class FilterDescriptorExpressionBuilder : FilterExpressionBuilder
    {
        // Fields
        private readonly FilterDescriptor descriptor;

        // Methods
        public FilterDescriptorExpressionBuilder(ParameterExpression parameterExpression, FilterDescriptor descriptor)
            : base(parameterExpression)
        {
            this.descriptor = descriptor;
        }

        public override Expression CreateBodyExpression()
        {
            Expression memberExpression = this.CreateMemberExpression();
            Type targetType = memberExpression.Type;
            Expression valueExpression = CreateValueExpression(targetType, this.descriptor.Value, CultureInfo.InvariantCulture);
            bool flag = true;
            if (TypesAreDifferent(this.descriptor, memberExpression, valueExpression))
            {
                if (!TryConvertExpressionTypes(ref memberExpression, ref valueExpression))
                {
                    flag = false;
                }
            }
            else if (memberExpression.Type.IsEnumType() || valueExpression.Type.IsEnumType())
            {
                if (!TryPromoteNullableEnums(ref memberExpression, ref valueExpression))
                {
                    flag = false;
                }
            }
            else if ((targetType.IsNullableType() && (memberExpression.Type != valueExpression.Type)) && !TryConvertNullableValue(memberExpression, ref valueExpression))
            {
                flag = false;
            }
            if (!flag)
            {
                object[] args = new object[] { this.descriptor.Operator, memberExpression.Type.GetTypeName(), valueExpression.Type.GetTypeName() };
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Operator '{0}' is incompatible with operand types '{1}' and '{2}'", args));
            }
            return this.descriptor.Operator.CreateExpression(memberExpression, valueExpression, false);
        }

        private static Expression CreateConstantExpression(object value)
        {
            if (value == null)
            {
                return ExpressionConstants.NullLiteral;
            }
            return Expression.Constant(value);
        }

        protected virtual Expression CreateMemberExpression()
        {
            Type memberType = this.FilterDescriptor.MemberType;
            Expression expression = ExpressionFactory.MakeMemberAccess(base.ParameterExpression, this.FilterDescriptor.Member);
            if ((memberType != null) && (expression.Type.GetNonNullableType() != memberType.GetNonNullableType()))
            {
                expression = Expression.Convert(expression, memberType);
            }
            return expression;
        }

        private static Expression CreateValueExpression(Type targetType, object value, CultureInfo culture)
        {
            if (((targetType != typeof(string)) && (!targetType.IsValueType || targetType.IsNullableType())) && (string.Compare(value as string, "null", StringComparison.OrdinalIgnoreCase) == 0))
            {
                value = null;
            }
            if (value != null)
            {
                Type nonNullableType = targetType.GetNonNullableType();
                if (value.GetType() != nonNullableType)
                {
                    if (nonNullableType.IsEnum)
                    {
                        value = Enum.Parse(nonNullableType, value.ToString(), true);
                    }
                    else if (nonNullableType == typeof(Guid))
                    {
                        value = new Guid(value.ToString());
                    }
                    else if (value is IConvertible)
                    {
                        value = Convert.ChangeType(value, nonNullableType, culture);
                    }
                }
            }
            return CreateConstantExpression(value);
        }

        private static Expression PromoteExpression(Expression expr, Type type, bool exact)
        {
            if (expr.Type == type)
            {
                return expr;
            }
            ConstantExpression expression = expr as ConstantExpression;
            if (((expression != null) && (expression == ExpressionConstants.NullLiteral)) && (!type.IsValueType || type.IsNullableType()))
            {
                return Expression.Constant(null, type);
            }
            if (!expr.Type.IsCompatibleWith(type))
            {
                return null;
            }
            if (!type.IsValueType && !exact)
            {
                return expr;
            }
            return Expression.Convert(expr, type);
        }

        private static bool TryConvertExpressionTypes(ref Expression memberExpression, ref Expression valueExpression)
        {
            if (memberExpression.Type != valueExpression.Type)
            {
                if (!memberExpression.Type.IsAssignableFrom(valueExpression.Type))
                {
                    if (!valueExpression.Type.IsAssignableFrom(memberExpression.Type))
                    {
                        return false;
                    }
                    memberExpression = Expression.Convert(memberExpression, valueExpression.Type);
                }
                else
                {
                    valueExpression = Expression.Convert(valueExpression, memberExpression.Type);
                }
            }
            return true;
        }

        private static bool TryConvertNullableValue(Expression memberExpression, ref Expression valueExpression)
        {
            ConstantExpression expression = valueExpression as ConstantExpression;
            if (expression != null)
            {
                try
                {
                    valueExpression = Expression.Constant(expression.Value, memberExpression.Type);
                }
                catch (ArgumentException)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool TryPromoteNullableEnums(ref Expression memberExpression, ref Expression valueExpression)
        {
            if (memberExpression.Type != valueExpression.Type)
            {
                Expression expression = PromoteExpression(valueExpression, memberExpression.Type, true);
                if (expression == null)
                {
                    expression = PromoteExpression(memberExpression, valueExpression.Type, true);
                    if (expression == null)
                    {
                        return false;
                    }
                    memberExpression = expression;
                }
                else
                {
                    valueExpression = expression;
                }
            }
            return true;
        }

        private static bool TypesAreDifferent(FilterDescriptor descriptor, Expression memberExpression, Expression valueExpression)
        {
            return ((((descriptor.Operator == FilterOperator.IsEqualTo) || (descriptor.Operator == FilterOperator.IsNotEqualTo)) && !memberExpression.Type.IsValueType) && !valueExpression.Type.IsValueType);
        }

        // Properties
        public FilterDescriptor FilterDescriptor
        {
            get
            {
                return this.descriptor;
            }
        }
    }
}
