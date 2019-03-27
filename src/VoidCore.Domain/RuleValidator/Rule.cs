using System;
using System.Collections.Generic;
using System.Linq;

namespace VoidCore.Domain.RuleValidator
{
    /// <summary>
    /// A rule for validating a request.
    /// </summary>
    /// <typeparam name="T">The type of request to validate.</typeparam>
    internal class Rule<T>
    {
        private readonly Func<T, IFailure> _failureBuilder;
        private readonly IReadOnlyList<Func<T, bool>> _invalidConditions;
        private readonly IReadOnlyList<Func<T, bool>> _suppressConditions;

        /// <summary>
        /// Construct a new rule and underlying validation error to throw when violations are detected.
        /// </summary>
        /// <param name="failureBuilder">A function that builds a custom IFailure to return if the rule fails.</param>
        /// <param name="invalidConditions">A set of functions that return true if the request is invalid.</param>
        /// <param name="suppressConditions">A set of functions that return true if the rule should be suppressed.</param>
        internal Rule(Func<T, IFailure> failureBuilder, IReadOnlyList<Func<T, bool>> invalidConditions, IReadOnlyList<Func<T, bool>> suppressConditions)
        {
            _failureBuilder = failureBuilder;
            _invalidConditions = invalidConditions;
            _suppressConditions = suppressConditions;
        }

        /// <summary>
        /// Run this rule to validate the request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IResult Run(T request)
        {
            return (!IsSuppressed(request) && IsInvalid(request)) ?
                Result.Fail(_failureBuilder(request)) :
                Result.Ok();
        }

        private bool IsInvalid(T request)
        {
            return _invalidConditions.Any() && _invalidConditions.Any(check => check(request));
        }

        private bool IsSuppressed(T request)
        {
            return _suppressConditions.Any() && _suppressConditions.Any(check => check(request));
        }
    }
}
