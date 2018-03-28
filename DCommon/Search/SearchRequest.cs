using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DCommon.Search
{
    public class SearchRequest
    {
        public List<FilterInput> Search { get; set; }
        public List<SortItem> Sort { get; set; }

        public void Update(SearchCriteria criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException("criteria");

            if (this.Search != null)
            {
                var filters = this.Search.Select(ToDescriptor).Where(d => d != null);
                criteria.FilterDescriptors.AddRange(filters);
            }
            if (this.Sort != null)
            {
                var sorts = this.Sort.Select(ToDescriptor).Where(d => d != null);
                criteria.SortDescriptors.AddRange(sorts);
            }
        }

        private static FilterDescriptor ToDescriptor(FilterItem filter)
        {
            if (string.IsNullOrEmpty(filter.Value) ||
                string.IsNullOrEmpty(filter.Op) ||
                string.IsNullOrEmpty(filter.Field))
                return null;

            var desc = new FilterDescriptor();
            desc.Operator = ParseOperator(filter.Op);
            desc.Member = filter.Field;
            desc.Value = filter.Value;
            return desc;
        }

        private static IFilterDescriptor ToDescriptor(FilterInput filter)
        {
            FilterLogicalOperator logicalOper = ParseLogicalOperator(filter.LogicalOp);
            var descriptors = filter.Filters.Select(ToDescriptor).Cast<IFilterDescriptor>().Where(d => d != null).ToList();

            if (filter.Children != null && filter.Children.Count > 0)
                descriptors.AddRange(filter.Children.Select(ToDescriptor).Where(d => d != null));

            if (descriptors.Count == 0)
                return null;

            if (descriptors.Count == 1)
                return descriptors[0];

            var c = new CompositeFilterDescriptor();
            c.LogicalOperator = logicalOper;
            c.FilterDescriptors.AddRange(descriptors);
            return c;
        }

        private static ISortDescriptor ToDescriptor(SortItem sortItem)
        {
            if (sortItem == null || string.IsNullOrWhiteSpace(sortItem.Field))
                return null;

            return new SortDescriptor
            {
                Direction = sortItem.Direction,
                Member = sortItem.Field,
            };
        }

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
        /// <param name="op"></param>
        /// <returns></returns>
        private static FilterOperator ParseOperator(string op)
        {
            switch (op)
            {
                case "eq": return FilterOperator.IsEqualTo;
                case "ne": return FilterOperator.IsNotEqualTo;
                case "lt": return FilterOperator.IsLessThan;
                case "le": return FilterOperator.IsLessThanOrEqualTo;
                case "gt": return FilterOperator.IsGreaterThan;
                case "ge": return FilterOperator.IsGreaterThanOrEqualTo;
                case "in": return FilterOperator.IsIn;
                case "ni": return FilterOperator.IsNotIn;
                case "bw": return FilterOperator.StartsWith;
                case "bn": return FilterOperator.NotStartsWith;
                case "ew": return FilterOperator.EndsWith;
                case "en": return FilterOperator.NotEndsWith;
                case "cn": return FilterOperator.Contains;
                case "nc": return FilterOperator.NotContains;
                default: return FilterOperator.IsEqualTo;
            }
        }

        /// <summary>
        /// or and
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        private static FilterLogicalOperator ParseLogicalOperator(string op)
        {
            return "OR".Equals(op, StringComparison.OrdinalIgnoreCase)
                ? FilterLogicalOperator.Or
                : FilterLogicalOperator.And;
        }
    }

    public class FilterInput
    {
        public string LogicalOp { get; set; }
        public List<FilterItem> Filters { get; set; }
        public List<FilterInput> Children { get; set; }
    }

    public class FilterItem
    {
        public string Value { get; set; }
        public string Field { get; set; }
        public string Op { get; set; }
    }

    public class SortItem
    {
        public string Field { get; set; }
        public ListSortDirection Direction { get; set; }
    }
}