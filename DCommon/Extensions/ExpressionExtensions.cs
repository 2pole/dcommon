using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace DCommon
{
    /// <summary>
    /// Defines extension method for <see cref="Expression"/>
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Merges two expressions together.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="merge"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        /// <summary>
        /// Creates an 'and' expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        /// <summary>
        /// Creates an 'or' expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }

        public static Expression MakeMemberAccess(this Expression instance, string memberName)
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

        #region Nested type: ParameterRebinder

        /// <inheritdoc />
        class ParameterRebinder : ExpressionVisitor
        {
            readonly Dictionary<ParameterExpression, ParameterExpression> map;

            ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map) { this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>(); }

            /// <summary>
            /// replaces the parameter expressions.
            /// </summary>
            /// <param name="map"></param>
            /// <param name="exp"></param>
            /// <returns></returns>
            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }

            /// <inheritdoc />
            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;

                if (map.TryGetValue(p, out replacement))
                    p = replacement;

                return base.VisitParameter(p);
            }
        }

        #endregion
    }
}
