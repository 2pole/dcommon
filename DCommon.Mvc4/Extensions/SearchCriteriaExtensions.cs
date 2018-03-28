using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DCommon.Mvc.Grid;
using DCommon.Search;

namespace DCommon.Mvc
{
    public static class SearchCriteriaExtensions
    {
        public static GridSortOptions ToSortOptions(this SearchCriteria criteria)
        {
            if (criteria == null || criteria.SortDescriptors.Count == 0)
                return null;

            var sortDescriptor = criteria.SortDescriptors.First();
            return new GridSortOptions()
                       {
                           Column = sortDescriptor.Member,
                           Direction = sortDescriptor.Direction
                       };
        }

        public static GridSortOptions ToSortOptions(this SearchCriteria criteria, GridSortOptions defaultSortOptions)
        {
            var sortOptions = criteria.ToSortOptions();
            return sortOptions ?? defaultSortOptions;
        }

        public static GridSortOptions ToSortOptions(this SearchCriteria criteria, string sortMember)
        {
            return criteria.ToSortOptions(sortMember, ListSortDirection.Descending);
        }

        public static GridSortOptions ToSortOptions(this SearchCriteria criteria, string sortMember, ListSortDirection sortDirection)
        {
            var defaultSortOptions = new GridSortOptions() { Column = sortMember, Direction = sortDirection };
            return criteria.ToSortOptions(defaultSortOptions);
        }
    }
}
