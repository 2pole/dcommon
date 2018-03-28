using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DCommon.Search.Expressions
{
    internal class FilterDescriptorCollectionExpressionBuilder : FilterExpressionBuilder
    {
        private readonly IEnumerable<IFilterDescriptor> filterDescriptors;
        private readonly FilterLogicalOperator logicalOperator;

        public FilterDescriptorCollectionExpressionBuilder(ParameterExpression parameterExpression, IEnumerable<IFilterDescriptor> filterDescriptors)
            : this(parameterExpression, filterDescriptors, FilterLogicalOperator.And)
        {
        }

        public FilterDescriptorCollectionExpressionBuilder(ParameterExpression parameterExpression, IEnumerable<IFilterDescriptor> filterDescriptors, FilterLogicalOperator logicalOperator)
            : base(parameterExpression)
        {
            this.filterDescriptors = filterDescriptors;
            this.logicalOperator = logicalOperator;
        }

        private static Expression ComposeExpressions(Expression left, Expression right, FilterLogicalOperator logicalOperator)
        {
            FilterLogicalOperator op = logicalOperator;
            if ((op != FilterLogicalOperator.And) && (op == FilterLogicalOperator.Or))
            {
                return Expression.OrElse(left, right);
            }
            return Expression.AndAlso(left, right);
        }

        public override Expression CreateBodyExpression()
        {
            Expression left = null;
            foreach (IFilterDescriptor descriptor in this.filterDescriptors)
            {
                Expression right = descriptor.CreateFilterExpression(base.ParameterExpression);
                left = left == null 
                    ? right 
                    : ComposeExpressions(left, right, this.logicalOperator);
            }
            if (left == null)
            {
                return ExpressionConstants.TrueLiteral;
            }
            return left;
            //return  Expression.Equal(left, Expression.Constant(true));
        }
    }
}
