using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VoidCore.Model.Data;
using VoidCore.Model.Responses.Collections;
using Xunit;

namespace VoidCore.Test.Model.Data
{
    public class QuerySpecificationAbstractTests
    {
        private static readonly Expression<Func<MyObject, bool>> _criteria1 = o => o.Include == "hi";
        private static readonly Expression<Func<MyObject, bool>> _criteria2 = o => o.ThenBy == "hi";
        private static readonly Expression<Func<MyObject, object>> _include = o => o.Include;
        private static readonly string _includeString = "include me";
        private static readonly Expression<Func<MyObject, object>> _orderBy = o => o.OrderBy;
        private static readonly Expression<Func<MyObject, object>> _orderByDesc = o => o.OrderByDescending;
        private static readonly Expression<Func<MyObject, object>> _thenBy = o => o.ThenBy;
        private static readonly Expression<Func<MyObject, object>> _thenByDesc = o => o.ThenByDescending;
        private static readonly PaginationOptions _paginationOptions = new(2, 3);

        [Fact]
        public void QuerySpecificationAbstract_sets_properties_from_methods()
        {
            var spec = new TestQuerySpecification(_criteria1);

            Assert.Equal(_criteria1, spec.Criteria[0]);
            Assert.Equal(_criteria2, spec.Criteria[1]);
            Assert.Equal(_include, spec.Includes.Single());
            Assert.Equal(_includeString, spec.IncludeStrings.Single());

            var actualOrderings = new HashSet<(Expression<Func<MyObject, object>> OrderBy, bool IsDescending)>(spec.Orderings);
            var expectedOrderings = new HashSet<(Expression<Func<MyObject, object>> OrderBy, bool IsDescending)>() {
                (_orderBy, false),
                (_orderByDesc, true),
                (_thenBy, false),
                (_thenByDesc, true)
            };

            Assert.Equal(expectedOrderings, actualOrderings);

            Assert.Equal(_paginationOptions, spec.PaginationOptions);
        }

        private class TestQuerySpecification : QuerySpecificationAbstract<MyObject>
        {
            public TestQuerySpecification(params Expression<Func<MyObject, bool>>[] criteria) : base(criteria)
            {
                AddCriteria(_criteria2);
                AddInclude(_include);
                AddInclude(_includeString);
                AddOrderBy(_orderBy);
                AddOrderBy(_orderByDesc, true);
                AddOrderBy(_thenBy);
                AddOrderBy(_thenByDesc, true);
                ApplyPaging(_paginationOptions);
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
