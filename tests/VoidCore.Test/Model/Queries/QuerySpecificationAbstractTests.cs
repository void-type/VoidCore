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
            Expression<Func<MyObject, bool>> criteria = o => o.Include == "hi";

            var spec = new TestQuerySpecification(
                include,
                includeString,
                orderBy,
                orderByDesc,
                criteria
            );

            Assert.Equal(criteria, spec.Criteria.Single());
            Assert.Equal(include, spec.Includes.Single());
            Assert.Equal(includeString, spec.IncludeStrings.Single());
            Assert.Equal(orderBy, spec.OrderBy);
            Assert.Equal(orderByDesc, spec.OrderByDescending);
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
                params Expression<Func<MyObject, bool>>[] criteria) : base(criteria)
            {
                AddInclude(include);
                AddInclude(includeString);
                ApplyOrderBy(orderBy);
                ApplyOrderByDescending(orderByDesc);
                ApplyPaging(2, 3);
            }
        }

        private class MyObject
        {
            public MyObject(string include, string orderBy, string orderByDescending)
            {
                Include = include;
                OrderBy = orderBy;
                OrderByDescending = orderByDescending;
            }

            public string Include { get; }
            public string OrderBy { get; }
            public string OrderByDescending { get; }
        }
    }
}
