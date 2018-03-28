using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DCommon.Search.Expressions
{
    internal static class ExpressionFactory
    {
        // Methods
        private static Expression CreateConditionExpression(Expression instance, Expression memberAccess, Expression defaultValue)
        {
            Expression right = DefaltValueExpression(instance.Type);
            return Expression.Condition(Expression.NotEqual(instance, right), memberAccess, defaultValue);
        }

        private static Expression CreateIfNullExpression(Expression instance, Expression memberAccess, Expression defaultValue)
        {
            if (ShouldGenerateCondition(instance.Type))
            {
                return CreateConditionExpression(instance, memberAccess, defaultValue);
            }
            return memberAccess;
        }

        public static Expression DefaltValueExpression(Type type)
        {
            return Expression.Constant(type.DefaultValue(), type);
        }

        private static Expression ExtractMemberAccessExpressionFromLiftedExpression(Expression liftedToNullExpression)
        {
            while (liftedToNullExpression.NodeType == ExpressionType.Conditional)
            {
                ConditionalExpression expression = (ConditionalExpression)liftedToNullExpression;
                if (expression.Test.NodeType == ExpressionType.NotEqual)
                {
                    liftedToNullExpression = expression.IfTrue;
                }
                else
                {
                    liftedToNullExpression = expression.IfFalse;
                }
            }
            return liftedToNullExpression;
        }

        private static Expression GetInstanceExpressionFromExpression(Expression memberAccess)
        {
            MemberExpression expression = memberAccess as MemberExpression;
            if (expression != null)
            {
                return expression.Expression;
            }
            MethodCallExpression expression2 = memberAccess as MethodCallExpression;
            if (expression2 != null)
            {
                return expression2.Object;
            }
            return null;
        }

        internal static bool IsNotNullConstantExpression(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Constant)
            {
                ConstantExpression expression2 = (ConstantExpression)expression;
                return (expression2.Value != null);
            }
            return false;
        }

        public static Expression LiftMemberAccessToNull(Expression memberAccess)
        {
            Expression defaultValue = DefaltValueExpression(memberAccess.Type);
            return LiftMemberAccessToNullRecursive(memberAccess, memberAccess, defaultValue);
        }

        private static Expression LiftMemberAccessToNullRecursive(Expression memberAccess, Expression conditionalExpression, Expression defaultValue)
        {
            Expression instanceExpressionFromExpression = GetInstanceExpressionFromExpression(memberAccess);
            if (instanceExpressionFromExpression == null)
            {
                return conditionalExpression;
            }
            conditionalExpression = CreateIfNullExpression(instanceExpressionFromExpression, conditionalExpression, defaultValue);
            return LiftMemberAccessToNullRecursive(instanceExpressionFromExpression, conditionalExpression, defaultValue);
        }

        public static Expression LiftMethodCallToNull(Expression instance, MethodInfo method, params Expression[] arguments)
        {
            return LiftMemberAccessToNull(Expression.Call(ExtractMemberAccessExpressionFromLiftedExpression(instance), method, arguments));
        }

        internal static Expression LiftStringExpressionToEmpty(Expression stringExpression)
        {
            if (stringExpression.Type != typeof(string))
            {
                throw new ArgumentException("Provided expression should have string type", "stringExpression");
            }
            if (IsNotNullConstantExpression(stringExpression))
            {
                return stringExpression;
            }
            return Expression.Coalesce(stringExpression, EmptyStringExpression);
        }

        public static Expression MakeMemberAccess(Expression instance, string memberName)
        {
            foreach (var token in memberName.Split('.'))
            {
                var memberInfo = instance.Type.FindPropertyOrField(token);
                if (memberInfo == null)
                    throw new ArgumentException(string.Format("Invalid {0} - '{1}' for type: {2}", token, token, instance.Type.GetTypeName()));

                instance = Expression.MakeMemberAccess(instance, memberInfo);
            }
            return instance;
        }

        public static Expression MakeMemberAccess(Expression instance, string memberName, bool liftMemberAccessToNull)
        {
            Expression memberAccess = MakeMemberAccess(instance, memberName);
            if (liftMemberAccessToNull)
            {
                return LiftMemberAccessToNull(memberAccess);
            }
            return memberAccess;
        }

        private static bool ShouldGenerateCondition(Type type)
        {
            return (!type.IsValueType || type.IsNullableType());
        }

        public static ConstantExpression EmptyStringExpression
        {
            get
            {
                return Expression.Constant(string.Empty);
            }
        }

        public static ConstantExpression ZeroExpression
        {
            get
            {
                return Expression.Constant(0);
            }
        }
    }
}
