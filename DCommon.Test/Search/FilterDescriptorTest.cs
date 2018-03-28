using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCommon.Search;
using Xunit;

namespace DCommon.Tests.Search
{
    public class FilterDescriptorTest : TestFixtureBase<FilterDescriptorContext>
    {
        [Fact]
        public void CompositeFilterTest()
        {
            var request = new SearchRequest();
            request.Search = new List<FilterInput>();
            var rootFilter = new FilterInput();
            rootFilter.LogicalOp = "OR";
            rootFilter.Filters = new List<FilterItem>();
            rootFilter.Filters.Add(new FilterItem { Field = "name", Op = "cn", Value = "N" });
            rootFilter.Filters.Add(new FilterItem { Field = "displayname", Op = "cn", Value = "N" });
            rootFilter.Children = new List<FilterInput>();

            var c1 = new FilterInput();
            c1.LogicalOp = "AND";
            c1.Filters = new List<FilterItem>();
            c1.Filters.Add(new FilterItem { Field = "Age", Op = "gt", Value = "30" });
            c1.Filters.Add(new FilterItem { Field = "Name", Op = "cn", Value = "3" });
            rootFilter.Children.Add(c1);
            request.Search.Add(rootFilter);

            var criteria = new SearchCriteria<User>();
            request.Update(criteria);
            criteria.PageSize = 20000;

            var users = base.FixtureContext.Users;
            var query = users.AsQueryable().ToPagination(criteria);
            var items = query.ToList();
            Assert.NotEmpty(items);
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
        public bool Enable { get; set; }
        public string Remark { get; set; }
    }

    public class FilterDescriptorContext
    {
        public IList<User> Users { get; private set; }

        public FilterDescriptorContext()
        {
            this.Users = Enumerable.Range(0, 1000).Select(CreateUser).ToList();
        }

        private User CreateUser(int index)
        {
            var rdm = new Random(Guid.NewGuid().GetHashCode());
            var u = new User();
            u.Age = rdm.Next(10, 50);
            u.Birthday = DateTime.Now.AddYears(-u.Age);
            u.Enable = u.Age % 2 == 0;
            u.Remark = u.Birthday.ToString();
            u.Name = "Name" + u.Age.ToString();
            u.DisplayName = "DisplayName" + u.Age.ToString();
            return u;
        }
    }
}
