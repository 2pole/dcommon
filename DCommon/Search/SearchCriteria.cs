using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DCommon.Collections.Pagination;

namespace DCommon.Search
{
    public class SearchCriteria
    {
        public static int DefaultPageNumber = 0;
        public static int DefaultPageSize = 20;

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public IList<IFilterDescriptor> FilterDescriptors { get; set; }

        public IList<ISortDescriptor> SortDescriptors { get; set; }

        public SearchCriteria()
        {
            this.PageNumber = DefaultPageNumber;
            this.PageSize = DefaultPageSize;
            this.FilterDescriptors = new List<IFilterDescriptor>();
            this.SortDescriptors = new List<ISortDescriptor>();
        }

        public virtual SearchCriteria CopyTo(SearchCriteria target = null)
        {
            target = target ?? new SearchCriteria();
            target.PageNumber = this.PageNumber;
            target.PageSize = this.PageSize;
            target.FilterDescriptors = this.FilterDescriptors;
            target.SortDescriptors = this.SortDescriptors;
            return target;
        }

        public static TCriteria ParseFromJson<TCriteria>(string json)
            where TCriteria : SearchCriteria
        {
            var criteria = json.FromJson<TCriteria>();
            var request = json.FromJson<SearchRequest>();
            request.Update(criteria);
            return criteria;
        }
    }

    public class SearchCriteria<T> : SearchCriteria
    {
        public virtual IQueryable<T> Filter(IQueryable<T> source)
        {
            return source.Where(this.FilterDescriptors);
        }

        public virtual IQueryable<T> Order(IQueryable<T> source)
        {
            return source.OrderBy(this.SortDescriptors);
        }

        public virtual IPagination<T> ToPagination(IQueryable<T> source)
        {
            var filteredQuery = this.Filter(source);
            var orderedQuery = this.Order(filteredQuery);
            return orderedQuery.AsPagination(this.PageNumber, this.PageSize);
        }
    }
}
