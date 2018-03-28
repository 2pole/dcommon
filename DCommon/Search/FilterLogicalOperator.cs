using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCommon.Search
{
	public enum FilterLogicalOperator
	{
		/// <summary>
		/// Combines filters with logical AND.
		/// </summary>
		And,

		/// <summary>
		/// Combines filters with logical OR.
		/// </summary>
		Or
	}
}