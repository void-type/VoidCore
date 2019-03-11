using System;
using System.Linq;
using System.Linq.Expressions;
using VoidCore.Model.Queries;
using Xunit;

namespace VoidCore.Test.Model.Queries
{
    public class QuerySpecificationAbstractTests
    {
        [Fact]
        public void SpecificationBaseSetsProperties()
        {
            Expression<Func<MyObject, object>> include = o => o.Include;
            var includeString = "include me";
            Expression<Func<MyObject, object>> orderBy = o => o.OrderBy;
            Expression<Func<MyObject, object>> orderByDesc = o => o.OrderByDescending;
            Expression<Func<MyObject, object>> thenBy = o => o.ThenBy;
            Expression<Func<MyObject, object>> thenByDesc = o => o.ThenByDescending;
            Expression<Func<MyObject, bool>> criteria = o => o.Include == "hi";

            var spec = new TestQuerySpecification(
                include,
                includeString,
                orderBy,
                orderByDesc,
                thenBy,
                thenByDesc,
                criteria
            );

            Assert.Equal(criteria, spec.Criteria.Single());
            Assert.Equal(include, spec.Includes.Single());
            Assert.Equal(includeString, spec.IncludeStrings.Single());
            Assert.Equal(orderBy, spec.OrderBy);
            Assert.Equal(orderByDesc, spec.OrderByDescending);
            Assert.Equal(thenBy, spec.SecondaryOrderings.Single(s => s.IsDescending == false).ThenBy);
            Assert.Equal(thenByDesc, spec.SecondaryOrderings.Single(s => s.IsDescending == true).ThenBy);
            Assert.True(spec.IsPagingEnabled);
            Assert.Equal(2, spec.Page);
            Assert.Equal(3, spec.Take);
        }

        private class TestQuerySpecification : QuerySpecificationAbstract<MyObject>
        {
            public TestQuerySpecification(
                Expression<Func<MyObject, object>> include,
                string includeString,
                Expression<Func<MyObject, object>> orderBy,
                Expression<Func<MyObject, object>> orderByDesc,
                Expression<Func<MyObject, object>> thenBy,
                Expression<Func<MyObject, object>> thenByDesc,
                params Expression<Func<MyObject, bool>>[] criteria) : base(criteria)
            {
                AddInclude(include);
                AddInclude(includeString);
                ApplyOrderBy(orderBy);
                ApplyOrderByDescending(orderByDesc);
                AddThenBy(thenBy);
                AddThenByDescending(thenByDesc);
                ApplyPaging(2, 3);
            }
        }

        private class MyObject
        {
            public MyObject(string include, string orderBy, string orderByDescending, string thenBy, string thenByDescending)
            {
                Include = include;
                OrderBy = orderBy;
                OrderByDescending = orderByDescending;
                ThenBy = thenBy;
                ThenByDescending = thenByDescending;
            }

            public string Include { get; }
            public string OrderBy { get; }
            public string OrderByDescending { get; }
            public string ThenBy { get; }
            public string ThenByDescending { get; }
        }
    }
}
