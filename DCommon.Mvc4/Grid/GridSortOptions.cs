using System.ComponentModel;

namespace DCommon.Mvc.Grid
{
	/// <summary>
	/// Sorting information for use with the grid.
	/// </summary>
    public class GridSortOptions
	{
		public string Column { get; set; }
		public ListSortDirection Direction { get; set; }
	}
}