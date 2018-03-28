using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DCommon.Search.Expressions
{
    internal class ExpressionBuilderBase
    {
        // Fields
        private readonly Type itemType;
        private ParameterExpression parameterExpression;

        // Methods
        protected ExpressionBuilderBase(Type itemType)
        {
            this.itemType = itemType;
        }

        // Properties
        protected internal Type ItemType
        {
            get
            {
                return this.itemType;
            }
        }

        protected internal ParameterExpression ParameterExpression
        {
            get
            {
                if (this.parameterExpression == null)
                {
                    this.parameterExpression = Expression.Parameter(this.ItemType, "item");
                }
                return this.parameterExpression;
            }
            set
            {
                this.parameterExpression = value;
            }
        }
    }
}
