using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;

namespace DCommon.Search
{
	public interface IFilterDescriptor
	{
		Expression CreateFilterExpression(Expression instance);

		//FilterLogicalOperator LogicalOperator { get; }
	}
}