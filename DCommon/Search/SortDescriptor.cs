using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace DCommon.Search
{
	public class SortDescriptor : ISortDescriptor
	{
        public string Member
		{
			get;
			set;
		}

		public ListSortDirection Direction
		{
			get;
			set;
		}
	}
}