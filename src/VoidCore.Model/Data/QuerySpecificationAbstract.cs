using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VoidCore.Domain;
using VoidCore.Domain.Guards;
using VoidCore.Model.Responses.Collections;

namespace VoidCore.Model.Data
{
    /// <inheritdoc/>
    public abstract class QuerySpecificationAbstract<T> : IQuerySpecification<T>
    {
        private readonly List<Expression<Func<T, bool>>> _criteria = new List<Expression<Func<T, bool>>>();
        private readonly List<Expression<Func<T, object>>> _includes = new List<Expression<Func<T, object>>>();
        private readonly List<string> _includeStrings = new List<string>();
        private readonly List<(Expression<Func<T, object>> ThenBy, bool IsDescending)> _secondaryOrderings = new List<(Expression<Func<T, object>>, bool)>();

        /// <summary>
        /// Create a new query
        /// </summary>
        /// <param name="criteria">The filtering criteria</param>
        protected QuerySpecificationAbstract(params Expression<Func<T, bool>>[] criteria)
        {
            AddCriteria(criteria);
        }

        /// <inheritdoc/>
        public IReadOnlyList<Expression<Func<T, bool>>> Criteria => _criteria;

        /// <inheritdoc/>
        public IReadOnlyList<Expression<Func<T, object>>> Includes => _includes;

        /// <inheritdoc/>
        public IEnumerable<string> IncludeStrings => _includeStrings;

        /// <inheritdoc/>
        public Maybe<Expression<Func<T, object>>> OrderBy { get; private set; } = Maybe<Expression<Func<T, object>>>.None;

        /// <inheritdoc/>
        public Maybe<Expression<Func<T, object>>> OrderByDescending { get; private set; } = Maybe<Expression<Func<T, object>>>.None;

        /// <inheritdoc/>
        public IReadOnlyList<(Expression<Func<T, object>> ThenBy, bool IsDescending)> SecondaryOrderings => _secondaryOrderings;

        /// <inheritdoc/>
        public PaginationOptions PaginationOptions { get; private set; } = PaginationOptions.None;

        /// <summary>
        /// Add criteria that will be used everytime this specification is used.
        /// </summary>
        /// <param name="criteria">The filtering criteria</param>
        protected void AddCriteria(params Expression<Func<T, bool>>[] criteria)
        {
            _criteria.AddRange(criteria);
        }

        /// <summary>
        /// Add an include expression.
        /// </summary>
        /// <param name="includeSelector">A selector of the extended entities to include</param>
        protected void AddInclude(Expression<Func<T, object>> includeSelector)
        {
            includeSelector.EnsureNotNull(nameof(includeSelector));
            _includes.Add(includeSelector);
        }

        /// <summary>
        /// Add an include string.
        /// </summary>
        /// <param name="includeString">A string that can be used with reflection to find extended entities</param>
        protected void AddInclude(string includeString)
        {
            includeString.EnsureNotNullOrEmpty(nameof(includeString));
            _includeStrings.Add(includeString);
        }

        /// <summary>
        /// Apply primary sorting to the query.
        /// </summary>
        /// <param name="sortPropertySelector">A selector for the property to sort by</param>
        /// <param name="isDescending">Toggle descending sort</param>
        protected void ApplyOrderBy(Expression<Func<T, object>> sortPropertySelector, bool isDescending = false)
        {
            ValidateOrderBy(sortPropertySelector);

            if (isDescending)
            {
                ApplyOrderByDescending(sortPropertySelector);
            }
            else
            {
                OrderBy = sortPropertySelector;
            }
        }

        /// <summary>
        /// Apply a descending primary sort to the query.
        /// </summary>
        /// <param name="sortPropertySelector">A selector for the property to sort by</param>
        protected void ApplyOrderByDescending(Expression<Func<T, object>> sortPropertySelector)
        {
            ValidateOrderBy(sortPropertySelector);
            OrderByDescending = sortPropertySelector;
        }

        /// <summary>
        /// Enable pagination on the query.
        /// </summary>
        /// <param name="paginationOptions">Options to control pagination</param>
        protected void ApplyPaging(PaginationOptions paginationOptions)
        {
            paginationOptions.EnsureNotNull(nameof(paginationOptions));
            PaginationOptions = paginationOptions;
        }

        /// <summary>
        /// Apply secondary sorting to the query.
        /// </summary>
        /// <param name="sortPropertySelector">A selector for the property to sort by</param>
        /// <param name="isDescending">Toggle descending sort</param>
        protected void AddThenBy(Expression<Func<T, object>> sortPropertySelector, bool isDescending = false)
        {
            ValidateThenBy(sortPropertySelector);
            _secondaryOrderings.Add((sortPropertySelector, isDescending));
        }

        /// <summary>
        /// Apply a secondary descending sort to the query.
        /// </summary>
        /// <param name="sortPropertySelector">A selector for the property to sort by</param>
        protected void AddThenByDescending(Expression<Func<T, object>> sortPropertySelector)
        {
            ValidateThenBy(sortPropertySelector);
            _secondaryOrderings.Add((sortPropertySelector, true));
        }

        private void ValidateOrderBy(Expression<Func<T, object>> sortPropertySelector)
        {
            sortPropertySelector.EnsureNotNull(nameof(sortPropertySelector));

            if (OrderBy.HasValue || OrderByDescending.HasValue)
            {
                throw new InvalidOperationException("Cannot apply multiple primary orderings to a specification.");
            }
        }

        private void ValidateThenBy(Expression<Func<T, object>> sortPropertySelector)
        {
            sortPropertySelector.EnsureNotNull(nameof(sortPropertySelector));

            if (!OrderBy.HasValue && !OrderByDescending.HasValue)
            {
                throw new InvalidOperationException("Cannot apply secondary orderings to a specification without a primary ordering.");
            }
        }
    }
}
