using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DCommon.Data;

namespace DCommon.Search
{
    public class EntitySearchCriteria<T> : SearchCriteria<T> where T : IEntity
    {
        public const string DefaultSortField = "Id";

        public string Key { get; set; }
        
        public virtual ISortDescriptor DefaultSortDescriptor
        {
            get { return new SortDescriptor { Member = DefaultSortField, Direction = ListSortDirection.Descending }; }
        }

        public override IQueryable<T> Order(IQueryable<T> source)
        {
            return base.SortDescriptors.Count == 0 ? source.OrderBy(DefaultSortDescriptor) : base.Order(source);
        }
    }
}
