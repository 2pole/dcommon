using System.Collections.Generic;
using DCommon.Mvc.Grid.Syntax;

namespace DCommon.Mvc.Grid
{
	/// <summary>
	/// Defines a grid model
	/// </summary>
	public interface IGridModel<T> where T: class 
	{
		IGridRenderer<T> Renderer { get; set; }
		IList<GridColumn<T>> Columns { get; }
		IGridSections<T> Sections { get; }
		string EmptyText { get; set; }
		IDictionary<string, object> Attributes { get; set; }
        IDictionary<string, object> BodyAttributes { get; set; }
        GridSortOptions SortOptions { get; set; }
		string SortPrefix { get; set; }
        bool AllowPage { get; set; }
	}
}