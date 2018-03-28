//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Linq.Expressions;
//using System.ComponentModel;

//namespace DCommon.Search
//{
//    public class ExpressionBuilder
//    {
//        public static Expression<Func<T, bool>> BuildFilters<T>(IEnumerable<IFilterDescriptor> filterDescriptors)
//        {			
//            if (filterDescriptors != null && filterDescriptors.Any())
//            {
//                Expression resultExpr = null;
//                ParameterExpression parameter = Expression.Parameter(typeof(T), "item");
//                foreach (var filter in filterDescriptors)
//                {
//                    var rightExpr = filter.CreateFilterExpression(parameter);
//                    if (rightExpr == null)
//                        continue;

//                    if (resultExpr != null)
//                    {
//                        switch (filter.LogicalOperator)
//                        {
//                            case FilterLogicalOperator.And:
//                                resultExpr = Expression.And(resultExpr, rightExpr);
//                                break;
//                            case FilterLogicalOperator.Or:
//                                resultExpr = Expression.Or(resultExpr, rightExpr);
//                                break;
//                        }
//                    }
//                    else
//                    {
//                        resultExpr = rightExpr;
//                    }
//                }
//                if (resultExpr != null)
//                {
//                    var lambda = Expression.Lambda<Func<T, bool>>(resultExpr, parameter);
//                    return lambda;
//                }
//            }
//            return null;
//        }
		
//        public static IQueryable<T> BuildSorts<T>(IQueryable<T> queryable, IEnumerable<ISortDescriptor> sortDescriptors)
//        {
//            if (sortDescriptors != null && sortDescriptors.Any())
//            {
//                ParameterExpression parameter = Expression.Parameter(typeof(T), "item");				
//                foreach (var sort in sortDescriptors)
//                {
//                    queryable = OrderBy(queryable, sort, parameter);
//                }				
//            }
//            return queryable;
//        }

//        private static IQueryable<T> OrderBy<T>(IQueryable<T> queryable, ISortDescriptor sortDesc, ParameterExpression parameter)
//        {
//            string methodName = sortDesc.Direction == ListSortDirection.Ascending ? "OrderBy" : "OrderByDescending";
//            MemberExpression memberAccess = null;
//            foreach (var property in sortDesc.Member.Split('.'))
//            {
//                memberAccess = MemberExpression.Property(memberAccess ?? (parameter as Expression), property);
//            }
//            LambdaExpression orderByLambda = Expression.Lambda(memberAccess, parameter);
//            MethodCallExpression result = Expression.Call(
//                    typeof(Queryable),
//                      methodName,
//                      new[] { queryable.ElementType, memberAccess.Type },
//                      queryable.Expression,
//                      Expression.Quote(orderByLambda));

//            return queryable.Provider.CreateQuery<T>(result);
//        }
//    }
//}