using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DCommon.Search.Expressions
{
    internal class SortDescriptorCollectionExpressionBuilder
    {
        private readonly IEnumerable<ISortDescriptor> sortDescriptors;

        // Methods
        public SortDescriptorCollectionExpressionBuilder(IEnumerable<ISortDescriptor> sortDescriptors)
        {
            this.sortDescriptors = sortDescriptors;
        }

        public IQueryable<T> Sort<T>(IQueryable<T> source)
        {
            bool flag = true;
            ParameterExpression parameter = Expression.Parameter(source.ElementType);
            foreach (ISortDescriptor descriptor in this.sortDescriptors)
            {
                var memberAccess = ExpressionFactory.MakeMemberAccess(parameter, descriptor.Member);
                LambdaExpression expression = Expression.Lambda(memberAccess, parameter);
                string methodName = string.Empty;
                if (flag)
                {
                    methodName = (descriptor.Direction != ListSortDirection.Ascending) ? "OrderByDescending" : "OrderBy";
                    flag = false;
                }
                else
                {
                    methodName = (descriptor.Direction != ListSortDirection.Ascending) ? "ThenByDescending" : "ThenBy";
                }

                Type[] typeArguments = new[] { source.ElementType, expression.Body.Type };
                Expression[] arguments = new[] { source.Expression, Expression.Quote(expression) };

                var method = Expression.Call(typeof(Queryable), methodName, typeArguments, arguments);
                source = source.Provider.CreateQuery<T>(method);
            }
            return source;
        }
    }
}
