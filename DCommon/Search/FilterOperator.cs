using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCommon.Search
{
	/// <summary>
	/// { op: "eq", text: "is equal to" },
	/// { op: "ne", text: "is not equal to" },
	/// { op: "lt", text: "is less than" },
	/// { op: "le", text: "is less or equal to" },
	/// { op: "gt", text: "is greater than" },
	/// { op: "ge", text: "is greater or equal to" },
	/// { op: "in", text: "is in" },
	/// { op: "ni", text: "is not in" },
	/// { op: "bw", text: "begins with" },
	/// { op: "bn", text: "does not begin with" },
	/// { op: "ew", text: "ends with" },
	/// { op: "en", text: "does not end with" },
	/// { op: "cn", text: "contains" },
	/// { op: "nc", text: "does not contain" }
	/// </summary>
	public enum FilterOperator
	{
		/// <summary>
		/// Left operand must be smaller than the right one.
		/// </summary>
		IsLessThan,
		/// <summary>
		/// Left operand must be smaller than or equal to the right one.
		/// </summary>
		IsLessThanOrEqualTo,
		/// <summary>
		/// Left operand must be equal to the right one.
		/// </summary>
		IsEqualTo,
		/// <summary>
		/// Left operand must be different from the right one.
		/// </summary>
		IsNotEqualTo,
		/// <summary>
		/// Left operand must be larger than the right one.
		/// </summary>
		IsGreaterThanOrEqualTo,
		/// <summary>
		/// Left operand must be larger than or equal to the right one.
		/// </summary>
		IsGreaterThan,
		/// <summary>
		/// Left operand must start with the right one.
		/// </summary>
		StartsWith,
		/// <summary>
		/// Left operand must not start with the right one.
		/// </summary>
		NotStartsWith,
		/// <summary>
		/// Left operand must end with the right one.
		/// </summary>
		EndsWith,
		/// <summary>
		/// Left operand must not end with the right one.
		/// </summary>
		NotEndsWith,
		/// <summary>
		/// Left operand must contain the right one.
		/// </summary>
		Contains,
		/// <summary>
		/// Left operand must not contain the right one.
		/// </summary>
		NotContains,
		/// <summary>
		/// Left operand must be contained in the right one.
		/// </summary>
		IsIn,
		/// <summary>
		/// Left operand must be not contained in the right one.
		/// </summary>
		IsNotIn
	}
}