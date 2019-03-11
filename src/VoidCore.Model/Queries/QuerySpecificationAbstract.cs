using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VoidCore.Model.Queries
{
    /// <inheritdoc/>
    public abstract class QuerySpecificationAbstract<T> : IQuerySpecification<T>
    {
        /// <summary>
        /// Create a new query
        /// </summary>
        /// <param name="criteria">The filtering criteria</param>
        protected QuerySpecificationAbstract(params Expression<Func<T, bool>>[] criteria)
        {
            Criteria = criteria;
        }

        /// <inheritdoc/>
        public Expression<Func<T, bool>>[] Criteria { get; }

        /// <inheritdoc/>
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        /// <inheritdoc/>
        public List<string> IncludeStrings { get; } = new List<string>();

        /// <inheritdoc/>
        public Expression<Func<T, object>> OrderBy { get; private set; }

        /// <inheritdoc/>
        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        /// <inheritdoc/>
        public List < (Expression<Func<T, object>> ThenBy, bool IsDescending) > SecondaryOrderings { get; } = new List < (Expression<Func<T, object>>, bool) > ();

        /// <inheritdoc/>
        public int Page { get; private set; }

        /// <inheritdoc/>
        public int Take { get; private set; }

        /// <inheritdoc/>
        public bool IsPagingEnabled { get; private set; }

        /// <summary>
        /// Add an include expression.
        /// </summary>
        /// <param name="includeExpression">A selector of the extended entities to include</param>
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        /// <summary>
        /// Add an include string
        /// </summary>
        /// <param name="includeString">A string that can be used with reflection to find extended entities</param>
        protected void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        /// <summary>
        /// Apply primary sorting to the query
        /// </summary>
        /// <param name="orderByExpression">A selector for the property to sort by</param>
        protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        /// <summary>
        /// Apply a descending primary sort to the query
        /// </summary>
        /// <param name="orderByDescendingExpression">A selector for the property to sort by</param>
        protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }

        /// <summary>
        /// Apply secondary sorting to the query
        /// </summary>
        /// <param name="sortExpression">A selector for the property to sort by</param>
        protected void AddThenBy(Expression<Func<T, object>> sortExpression)
        {
            SecondaryOrderings.Add((sortExpression, false));
        }

        /// <summary>
        /// Apply a secondary descending sort to the query
        /// </summary>
        /// <param name="sortExpression">A selector for the property to sort by</param>
        protected void AddThenByDescending(Expression<Func<T, object>> sortExpression)
        {
            SecondaryOrderings.Add((sortExpression, true));
        }

        /// <summary>
        /// /// Enable pagination on the query
        /// </summary>
        /// <param name="page">The page number</param>
        /// <param name="take">The size of the pages</param>
        protected void ApplyPaging(int page, int take)
        {
            Page = page;
            Take = take;
            IsPagingEnabled = true;
        }
    }
}
