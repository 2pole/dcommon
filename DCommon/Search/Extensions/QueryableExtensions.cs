using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DCommon.Collections.Pagination;
using DCommon.Search.Expressions;

namespace DCommon.Search
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, ISortDescriptor sortDescriptor)
        {
            return query.OrderBy(new[] { sortDescriptor });
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> query, IEnumerable<IFilterDescriptor> filterDescriptors)
        {
            //Expression<Func<T, bool>> whereLambda = ExpressionBuilder.BuildFilters<T>(filterDescriptors);
            //if (null != whereLambda)
            //{
            //    return query.Where(whereLambda);
            //}
            //return query;
            if (filterDescriptors != null && filterDescriptors.Any())
            {
                var parameter = Expression.Parameter(query.ElementType, "item");
                var builder = new FilterDescriptorCollectionExpressionBuilder(parameter, filterDescriptors);
                LambdaExpression predicate = builder.CreateFilterExpression();
                Expression<Func<T, bool>> expression = predicate as Expression<Func<T, bool>>;
                if (expression != null)
                    return query.Where<T>(expression);
            }
            return query;
        }

        public static IQueryable Where(this IQueryable source, Expression predicate)
        {
            Type[] typeArguments = new Type[] { source.ElementType };
            Expression[] arguments = new Expression[] { source.Expression, Expression.Quote(predicate) };
            return source.Provider.CreateQuery(Expression.Call(typeof(Queryable), "Where", typeArguments, arguments));
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, IEnumerable<ISortDescriptor> sortDescriptors)
        {
            var builder = new SortDescriptorCollectionExpressionBuilder(sortDescriptors);
            var orderedQuery = builder.Sort(query);
            return orderedQuery;
            //return ExpressionBuilder.BuildSorts(query, sortDescriptors);
        }

        public static IPagination<T> ToPagination<T>(this IQueryable<T> query, SearchCriteria<T> criteria)
        {
            return criteria.ToPagination(query);
        }
    }
}
