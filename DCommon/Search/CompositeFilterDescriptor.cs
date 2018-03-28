using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using DCommon.Search.Expressions;

namespace DCommon.Search
{
	public class CompositeFilterDescriptor : FilterDescriptorBase
	{
        public FilterLogicalOperator LogicalOperator
        {
            get;
            set;
        }

		public CompositeFilterDescriptor()
		{
			FilterDescriptors = new FilterDescriptorCollection();
		}

		public FilterDescriptorCollection FilterDescriptors
		{
			get;
			private set;
		}

        protected override Expression CreateFilterExpression(ParameterExpression parameter)
		{
            var builder = new FilterDescriptorCollectionExpressionBuilder(parameter, this.FilterDescriptors, this.LogicalOperator);
            return builder.CreateBodyExpression();
		}
	}
}