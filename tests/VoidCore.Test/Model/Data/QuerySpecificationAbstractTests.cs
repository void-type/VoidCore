using System;
using System.Linq;
using System.Linq.Expressions;
using VoidCore.Domain;
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
        private static readonly PaginationOptions _paginationOptions = new PaginationOptions(2, 3);

        [Fact]
        public void QuerySpecificationAbstract_sets_properties_from_methods()
        {
            var spec = new TestQuerySpecification(_criteria1);

            Assert.Equal(_criteria1, spec.Criteria[0]);
            Assert.Equal(_criteria2, spec.Criteria[1]);
            Assert.Equal(_include, spec.Includes.Single());
            Assert.Equal(_includeString, spec.IncludeStrings.Single());
            Assert.Equal(_orderBy, spec.OrderBy.Value);
            Assert.Equal(Maybe<Expression<Func<MyObject, object>>>.None, spec.OrderByDescending);
            Assert.Equal(_thenBy, spec.SecondaryOrderings[0].ThenBy);
            Assert.Equal(_thenByDesc, spec.SecondaryOrderings[1].ThenBy);
            Assert.Equal(_paginationOptions, spec.PaginationOptions);
        }

        private class TestQuerySpecification : QuerySpecificationAbstract<MyObject>
        {
            public TestQuerySpecification(params Expression<Func<MyObject, bool>>[] criteria) : base(criteria)
            {
                AddCriteria(_criteria2);
                AddInclude(_include);
                AddInclude(_includeString);
                ApplyOrderBy(_orderBy);
                AddThenBy(_thenBy);
                AddThenByDescending(_thenByDesc);
                ApplyPaging(_paginationOptions);
            }
        }

        [Fact]
        public void QuerySpecificationAbstract_throws_exception_when_orderBy_and_orderByDescending_are_both_applied()
        {
            Assert.Throws<InvalidOperationException>(() => new TestQuerySpecificationInvalidOrderings1(_criteria1));
        }

        private class TestQuerySpecificationInvalidOrderings1 : QuerySpecificationAbstract<MyObject>
        {
            public TestQuerySpecificationInvalidOrderings1(params Expression<Func<MyObject, bool>>[] criteria) : base(criteria)
            {
                ApplyOrderBy(_orderBy);
                ApplyOrderByDescending(_orderByDesc);
            }
        }

        [Fact]
        public void QuerySpecificationAbstract_throws_exception_when_orderBy_called_twice()
        {
            Assert.Throws<InvalidOperationException>(() => new TestQuerySpecificationInvalidOrderings2(_criteria1));
        }

        private class TestQuerySpecificationInvalidOrderings2 : QuerySpecificationAbstract<MyObject>
        {
            public TestQuerySpecificationInvalidOrderings2(params Expression<Func<MyObject, bool>>[] criteria) : base(criteria)
            {
                ApplyOrderBy(_orderBy);
                ApplyOrderBy(_orderBy);
            }
        }

        [Fact]
        public void QuerySpecificationAbstract_throws_exception_when_thenBy_called_without_orderBy()
        {
            Assert.Throws<InvalidOperationException>(() => new TestQuerySpecificationInvalidOrderings3(_criteria1));
            Assert.Throws<InvalidOperationException>(() => new TestQuerySpecificationInvalidOrderings4(_criteria1));
        }

        private class TestQuerySpecificationInvalidOrderings3 : QuerySpecificationAbstract<MyObject>
        {
            public TestQuerySpecificationInvalidOrderings3(params Expression<Func<MyObject, bool>>[] criteria) : base(criteria)
            {
                AddThenBy(_thenBy);
            }
        }

        private class TestQuerySpecificationInvalidOrderings4 : QuerySpecificationAbstract<MyObject>
        {
            public TestQuerySpecificationInvalidOrderings4(params Expression<Func<MyObject, bool>>[] criteria) : base(criteria)
            {
                AddThenByDescending(_thenByDesc);
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
