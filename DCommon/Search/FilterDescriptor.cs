using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using DCommon.Search.Expressions;

namespace DCommon.Search
{
    public class FilterDescriptor : FilterDescriptorBase, IEquatable<FilterDescriptor>
	{
        public FilterDescriptor()
            : this(string.Empty, FilterOperator.IsEqualTo, null)
        {
        }

        public FilterDescriptor(string member, FilterOperator filterOperator, string filterValue)
        {
            this.Member = member;
            this.Operator = filterOperator;
            this.Value = filterValue;
        }

		/// <summary>
		/// Gets or sets the member name which will be used for filtering.
		/// </summary>
		/// <filterValue>The member that will be used for filtering.</filterValue>
		public string Member
		{
			get;
			set;
		}

        public Type MemberType { get; set; }

		/// <summary>
		/// Gets or sets the filter operator.
		/// </summary>
		/// <filterValue>The filter operator.</filterValue>
		public FilterOperator Operator
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the target filter value.
		/// </summary>
		/// <filterValue>The filter value.</filterValue>
		public string Value
		{
			get;
			set;
		}

        protected override Expression CreateFilterExpression(ParameterExpression parameter)
		{
            var builder = new FilterDescriptorExpressionBuilder(parameter, this);
            return builder.CreateBodyExpression();

            //if (string.IsNullOrEmpty(this.Value))
            //    return null;

            //MemberExpression memberAccess = null;
            //foreach (var property in this.Member.Split('.'))
            //{
            //    memberAccess = MemberExpression.Property(memberAccess ?? parameter, property);
            //}
            //var formatedValue = Convert.ChangeType(this.Value, memberAccess.Type);
            //ConstantExpression filter = Expression.Constant(formatedValue);			

            //Expression right = null;
            //switch (Operator)
            //{
            //    case FilterOperator.IsLessThan:
            //        right = Expression.LessThan(memberAccess, filter);
            //        break;
            //    case FilterOperator.IsLessThanOrEqualTo:
            //        right = Expression.LessThanOrEqual(memberAccess, filter);
            //        break;
            //    case FilterOperator.IsEqualTo:
            //        right = Expression.Equal(memberAccess, filter);
            //        break;
            //    case FilterOperator.IsNotEqualTo:
            //        right = Expression.NotEqual(memberAccess, filter);
            //        break;
            //    case FilterOperator.IsGreaterThan:
            //        right = Expression.GreaterThan(memberAccess, filter);
            //        break;
            //    case FilterOperator.IsGreaterThanOrEqualTo:
            //        right = Expression.GreaterThanOrEqual(memberAccess, filter);
            //        break;				
            //    case FilterOperator.StartsWith:					
            //        right = Expression.Call(memberAccess, StartsWithMethod, filter);
            //        break;
            //    case FilterOperator.NotStartsWith:
            //        right = Expression.Call(memberAccess, StartsWithMethod, filter);
            //        right = Expression.Equal(right, FalseConstant);
            //        break;
            //    case FilterOperator.EndsWith:
            //        right = Expression.Call(memberAccess, EndsWithMethod, filter);
            //        break;
            //    case FilterOperator.NotEndsWith:
            //        right = Expression.Call(memberAccess, EndsWithMethod, filter);
            //        right = Expression.Equal(right, FalseConstant);
            //        break;
            //    case FilterOperator.Contains:
            //        right = Expression.Call(memberAccess, ContainsMethod, filter);
            //        break;
            //    case FilterOperator.NotContains:
            //        right = Expression.Call(memberAccess, ContainsMethod, filter);
            //        right = Expression.Equal(right, FalseConstant);
            //        break;
            //    case FilterOperator.IsIn:
            //        break;
            //    case FilterOperator.IsNotIn:
            //        break;
            //    default:
            //        break;
            //}
            //return right;
		}

        #region IEquatable<FilterDescriptor>
        public virtual bool Equals(FilterDescriptor other)
        {
            if (object.ReferenceEquals(null, other))
                return false;
            
            return (object.ReferenceEquals(this, other) || ((other.Operator == this.Operator &&
                                                             other.Member == this.Member) && 
                                                             other.Value == this.Value));
        }
        #endregion

        public override bool Equals(object obj)
        {
            var other = obj as FilterDescriptor;
            if (other == null)
                return false;
            
            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            int num = (this.Operator.GetHashCode() * 0x18d) ^ ((this.Member == null) ? 0 : this.Member.GetHashCode());
            return ((num * 0x18d) ^ ((this.Value == null) ? 0 : this.Value.GetHashCode()));
        }
	}
}