using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DCommon.Collections.Pagination
{
	/// <summary>
	/// Implementation of IPagination that wraps a pre-paged data source. 
	/// </summary>
	public class CustomPagination<T> : IPagination<T>
	{
		private readonly IList<T> _dataSource;

		/// <summary>
		/// Creates a new instance of CustomPagination
		/// </summary>
		/// <param name="dataSource">A pre-paged slice of data</param>
		/// <param name="pageNumber">The current page number, Index from 0.</param>
		/// <param name="pageSize">The number of items per page</param>
		/// <param name="totalItems">The total number of items in the overall datasource</param>
		public CustomPagination(IEnumerable<T> dataSource, int pageNumber, int pageSize, int totalItems)
		{
			_dataSource = dataSource.ToList();
			PageNumber = pageNumber;
			PageSize = pageSize;
			TotalItems = totalItems;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _dataSource.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

        /// <summary>
        /// Index from 0.
        /// </summary>
		public int PageNumber { get; private set; }
		public int PageSize { get; private set; }
		public int TotalItems { get; private set; }

		public int TotalPages
		{
			get { return (int)Math.Ceiling(((double)TotalItems) / PageSize); }
		}

		public int FirstItem
		{
			get
			{
				return PageNumber * PageSize;
			}
		}

		public int LastItem
		{
			get { return FirstItem + _dataSource.Count - 1; }
		}

		public bool HasPreviousPage
		{
			get { return PageNumber > 0; }
		}

		public bool HasNextPage
		{
			get { return PageNumber < TotalPages - 1; }
		}
	}
}