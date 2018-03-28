using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DCommon.Search.Expressions
{
    internal abstract class FilterExpressionBuilder : ExpressionBuilderBase
    {
        // Methods
        protected FilterExpressionBuilder(ParameterExpression parameterExpression)
            : base(parameterExpression.Type)
        {
            base.ParameterExpression = parameterExpression;
        }

        public LambdaExpression CreateFilterExpression()
        {
            ParameterExpression[] parameters = new ParameterExpression[] { base.ParameterExpression };
            return Expression.Lambda(this.CreateBodyExpression(), parameters);
        }

        public abstract Expression CreateBodyExpression();
    }
}
