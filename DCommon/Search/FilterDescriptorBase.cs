using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Reflection;

namespace DCommon.Search
{
	public abstract class FilterDescriptorBase : IFilterDescriptor
	{
		protected static readonly MethodInfo StartsWithMethod;
		protected static readonly MethodInfo EndsWithMethod;
		protected static readonly MethodInfo ContainsMethod;
		protected static readonly ConstantExpression FalseConstant;

		static FilterDescriptorBase()
		{
			StartsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
			EndsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });
			ContainsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
			FalseConstant = Expression.Constant(false);
		}

        public virtual Expression CreateFilterExpression(Expression instance)
        {
            var parameterExpression = instance as ParameterExpression;
            if (parameterExpression == null)
            {
                throw new ArgumentException("Parameter should be of type ParameterExpression", "instance");
            }
            return this.CreateFilterExpression(parameterExpression);
        }

        protected virtual Expression CreateFilterExpression(ParameterExpression parameterExpression)
        {
            return parameterExpression;
        }
	}
}