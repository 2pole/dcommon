using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DCommon.Search.Expressions
{
    internal static class FilterOperatorExtensions
    {
        // Fields
        internal static readonly MethodInfo StringCompareMethodInfo;
        internal static readonly MethodInfo StringContainsMethodInfo;
        internal static readonly MethodInfo StringEndsWithMethodInfo;
        internal static readonly MethodInfo StringStartsWithMethodInfo;
        internal static readonly MethodInfo StringToLowerMethodInfo = typeof(string).GetMethod("ToLower", new Type[0]);

        // Methods
        static FilterOperatorExtensions()
        {
            Type[] types = new Type[] { typeof(string) };
            StringStartsWithMethodInfo = typeof(string).GetMethod("StartsWith", types);
            Type[] typeArray2 = new Type[] { typeof(string) };
            StringEndsWithMethodInfo = typeof(string).GetMethod("EndsWith", typeArray2);
            Type[] typeArray3 = new Type[] { typeof(string), typeof(string) };
            StringCompareMethodInfo = typeof(string).GetMethod("Compare", typeArray3);
            Type[] typeArray4 = new Type[] { typeof(string) };
            StringContainsMethodInfo = typeof(string).GetMethod("Contains", typeArray4);
        }

        internal static Expression CreateExpression(this FilterOperator filterOperator, Expression left, Expression right, bool liftMemberAccess)
        {
            switch (filterOperator)
            {
                case FilterOperator.IsLessThan:
                    return GenerateLessThan(left, right, liftMemberAccess);

                case FilterOperator.IsLessThanOrEqualTo:
                    return GenerateLessThanEqual(left, right, liftMemberAccess);

                case FilterOperator.IsEqualTo:
                    return GenerateEqual(left, right, liftMemberAccess);

                case FilterOperator.IsNotEqualTo:
                    return GenerateNotEqual(left, right, liftMemberAccess);

                case FilterOperator.IsGreaterThanOrEqualTo:
                    return GenerateGreaterThanEqual(left, right, liftMemberAccess);

                case FilterOperator.IsGreaterThan:
                    return GenerateGreaterThan(left, right, liftMemberAccess);

                case FilterOperator.StartsWith:
                    return GenerateStartsWith(left, right, liftMemberAccess);

                case FilterOperator.NotStartsWith:
                    return GenerateNotStartsWith(left, right, liftMemberAccess);

                case FilterOperator.EndsWith:
                    return GenerateEndsWith(left, right, liftMemberAccess);

                case FilterOperator.NotEndsWith:
                    return GenerateNotEndsWith(left, right, liftMemberAccess);

                case FilterOperator.Contains:
                    return GenerateContains(left, right, liftMemberAccess);

                case FilterOperator.NotContains:
                    return GenerateNotContains(left, right, liftMemberAccess);

                //case FilterOperator.IsIn:
                //    return GenerateIsContainedIn(left, right, liftMemberAccess);

                //case FilterOperator.IsNotIn:
                //    return GenerateNotContains(left, right, liftMemberAccess);
            }
            throw new InvalidOperationException();
        }

        private static Expression GenerateCaseInsensitiveStringMethodCall(MethodInfo methodInfo, Expression left, Expression right, bool liftMemberAccess)
        {
            Expression instance = GenerateToLowerCall(left, liftMemberAccess);
            Expression expression2 = GenerateToLowerCall(right, liftMemberAccess);
            if (methodInfo.IsStatic)
            {
                Expression[] expressionArray1 = new Expression[] { instance, expression2 };
                return Expression.Call(methodInfo, expressionArray1);
            }
            Expression[] arguments = new Expression[] { expression2 };
            return Expression.Call(instance, methodInfo, arguments);
        }

        private static Expression GenerateContains(Expression left, Expression right, bool liftMemberAccess)
        {
            return Expression.Equal(GenerateCaseInsensitiveStringMethodCall(StringContainsMethodInfo, left, right, liftMemberAccess), ExpressionConstants.TrueLiteral);
        }

        private static Expression GenerateEndsWith(Expression left, Expression right, bool liftMemberAccess)
        {
            return Expression.Equal(GenerateCaseInsensitiveStringMethodCall(StringEndsWithMethodInfo, left, right, liftMemberAccess), ExpressionConstants.TrueLiteral);
        }

        private static Expression GenerateNotEndsWith(Expression left, Expression right, bool liftMemberAccess)
        {
            return Expression.Equal(GenerateCaseInsensitiveStringMethodCall(StringEndsWithMethodInfo, left, right, liftMemberAccess), ExpressionConstants.FalseLiteral);
        }

        private static Expression GenerateEqual(Expression left, Expression right, bool liftMemberAccess)
        {
            if (left.Type == typeof(string))
            {
                left = GenerateToLowerCall(left, liftMemberAccess);
                right = GenerateToLowerCall(right, liftMemberAccess);
            }
            return Expression.Equal(left, right);
        }

        private static Expression GenerateGreaterThan(Expression left, Expression right, bool liftMemberAccess)
        {
            if (left.Type == typeof(string))
            {
                return Expression.GreaterThan(GenerateCaseInsensitiveStringMethodCall(StringCompareMethodInfo, left, right, liftMemberAccess), ExpressionFactory.ZeroExpression);
            }
            return Expression.GreaterThan(left, right);
        }

        private static Expression GenerateGreaterThanEqual(Expression left, Expression right, bool liftMemberAccess)
        {
            if (left.Type == typeof(string))
            {
                return Expression.GreaterThanOrEqual(GenerateCaseInsensitiveStringMethodCall(StringCompareMethodInfo, left, right, liftMemberAccess), ExpressionFactory.ZeroExpression);
            }
            return Expression.GreaterThanOrEqual(left, right);
        }

        private static Expression GenerateIsContainedIn(Expression left, Expression right, bool liftMemberAccess)
        {
            return Expression.Equal(GenerateCaseInsensitiveStringMethodCall(StringContainsMethodInfo, right, left, liftMemberAccess), ExpressionConstants.TrueLiteral);
        }

        private static Expression GenerateLessThan(Expression left, Expression right, bool liftMemberAccess)
        {
            if (left.Type == typeof(string))
            {
                return Expression.LessThan(GenerateCaseInsensitiveStringMethodCall(StringCompareMethodInfo, left, right, liftMemberAccess), ExpressionFactory.ZeroExpression);
            }
            return Expression.LessThan(left, right);
        }

        private static Expression GenerateLessThanEqual(Expression left, Expression right, bool liftMemberAccess)
        {
            if (left.Type == typeof(string))
            {
                return Expression.LessThanOrEqual(GenerateCaseInsensitiveStringMethodCall(StringCompareMethodInfo, left, right, liftMemberAccess), ExpressionFactory.ZeroExpression);
            }
            return Expression.LessThanOrEqual(left, right);
        }

        private static Expression GenerateNotContains(Expression left, Expression right, bool liftMemberAccess)
        {
            return Expression.Equal(GenerateCaseInsensitiveStringMethodCall(StringContainsMethodInfo, left, right, liftMemberAccess), ExpressionConstants.FalseLiteral);
        }

        private static Expression GenerateNotEqual(Expression left, Expression right, bool liftMemberAccess)
        {
            if (left.Type == typeof(string))
            {
                left = GenerateToLowerCall(left, liftMemberAccess);
                right = GenerateToLowerCall(right, liftMemberAccess);
            }
            return Expression.NotEqual(left, right);
        }

        private static Expression GenerateStartsWith(Expression left, Expression right, bool liftMemberAccess)
        {
            return Expression.Equal(GenerateCaseInsensitiveStringMethodCall(StringStartsWithMethodInfo, left, right, liftMemberAccess), ExpressionConstants.TrueLiteral);
        }

        private static Expression GenerateNotStartsWith(Expression left, Expression right, bool liftMemberAccess)
        {
            return Expression.Equal(GenerateCaseInsensitiveStringMethodCall(StringStartsWithMethodInfo, left, right, liftMemberAccess), ExpressionConstants.FalseLiteral);
        }

        private static Expression GenerateToLowerCall(Expression stringExpression, bool liftMemberAccess)
        {
            if (liftMemberAccess)
            {
                stringExpression = ExpressionFactory.LiftStringExpressionToEmpty(stringExpression);
            }
            //return Expression.Call(stringExpression, StringToLowerMethodInfo);
            return stringExpression;
        }
    }
}
