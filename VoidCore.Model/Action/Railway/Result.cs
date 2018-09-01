using System;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Validation;

namespace VoidCore.Model.Action.Railway
{
    public class Result<TValue> : AbstractResult
    {
        public TValue Value { get; }

        internal Result(TValue value) : base(false, null)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value),
                    "Cannot set a result of null. Use non-generic Result for void results.");
            }
            Value = value;
        }

        internal Result(IEnumerable<IFailure> failures) : base(true, failures) { }
    }

    public class Result : AbstractResult
    {
        protected Result() : base(false, null) { }

        private Result(IEnumerable<IFailure> failures) : base(true, failures) { }

        public static Result Ok()
        {
            return new Result();
        }

        public static Result<TValue> Ok<TValue>(TValue success)
        {
            return new Result<TValue>(success);
        }

        public static Result Fail(IEnumerable<IFailure> failures)
        {
            return new Result(failures);
        }

        public static Result<TValue> Fail<TValue>(IEnumerable<IFailure> failures)
        {
            return new Result<TValue>(failures);
        }

        public static Result Fail(IFailure failure)
        {
            return Fail(new [] { failure });
        }

        public static Result<TValue> Fail<TValue>(IFailure failure)
        {
            return Fail<TValue>(new [] { failure });
        }

        public static Result Fail(string errorMessage, string uiHandle = null)
        {
            return Fail(new Failure(errorMessage, uiHandle));
        }

        public static Result<TValue> Fail<TValue>(string errorMessage, string uiHandle = null)
        {
            return Fail<TValue>(new Failure(errorMessage, uiHandle));
        }

        public static Result CombineFailures<TValue>(IEnumerable<Result> results)
        {
            var failedResults = results
                .Where(result => result.IsFailed)
                .SelectMany(result => result.Failures);

            return Fail(failedResults);
        }

        public static Result<TValue> CombineFailures<TValue>(IEnumerable<Result<TValue>> results)
        {
            var failedResults = results
                .Where(result => result.IsFailed)
                .SelectMany(result => result.Failures);

            return Fail<TValue>(failedResults);
        }
    }
}
