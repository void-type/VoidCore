﻿using System;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Guards;

namespace VoidCore.Model.RuleValidator
{
    /// <summary>
    /// The base for a custom rule-based request validator.
    /// </summary>
    /// <typeparam name="T">The type of entities to validate</typeparam>
    public abstract class RuleValidatorAbstract<T> : IRequestValidator<T>
    {
        private readonly List<RuleBuilder<T>> _ruleBuilders = new();

        /// <inheritdoc/>
        public IResult<T> Validate(T request)
        {
            request.EnsureNotNull(nameof(request));

            return _ruleBuilders
                .Select(builder => builder.Build().Run(request))
                .Combine()
                .Select(() => request);
        }

        /// <summary>
        /// Create a new rule for this request.
        /// </summary>
        /// <param name="failureBuilder">A builder function for the failure to give upon invalid request.</param>
        /// <returns>A rule builder to continue building this rule</returns>
        protected IRuleBuilder<T> CreateRule(Func<T, IFailure> failureBuilder)
        {
            var rule = new RuleBuilder<T>(failureBuilder);
            _ruleBuilders.Add(rule);
            return rule;
        }

        /// <summary>
        /// Create a new rule for this request.
        /// </summary>
        /// <param name="failure">The failure to give upon invalid request.</param>
        /// <returns>A rule builder to continue building this rule</returns>
        protected IRuleBuilder<T> CreateRule(IFailure failure)
        {
            return CreateRule(_ => failure);
        }
    }
}
