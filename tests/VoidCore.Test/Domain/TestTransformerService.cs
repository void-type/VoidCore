using System;
using System.Threading.Tasks;
using VoidCore.Domain;

namespace VoidCore.Test.Domain
{
    internal class TestTransformerService
    {
        public int LastStep { get; private set; }
        public string Start => "Hello World";

        public string Transform(string input, int currentStep)
        {
            if (LastStep != currentStep - 1)
            {
                throw new InvalidOperationException();
            }

            LastStep = currentStep;
            return input + "!";
        }

        public async Task<string> TransformAsync(string input, int currentStep)
        {
            if (LastStep != currentStep - 1)
            {
                throw new InvalidOperationException();
            }

            await Task.Delay(10);
            LastStep = currentStep;
            return input + "!";
        }

        public Maybe<string> TransformMaybe(string input, int currentStep)
        {
            return Transform(input, currentStep);
        }

        public async Task<Maybe<string>> TransformMaybeAsync(string input, int currentStep)
        {
            return await TransformAsync(input, currentStep);
        }

        public IResult GetResult(int currentStep, bool success = true)
        {
            if (LastStep != currentStep - 1)
            {
                throw new InvalidOperationException();
            }

            LastStep = currentStep;
            return success ? Result.Ok() : Result.Fail(new Failure("oops"));
        }

        public IResult<T> GetResult<T>(T obj, int currentStep, bool success = true)
        {
            if (LastStep != currentStep - 1)
            {
                throw new InvalidOperationException();
            }

            LastStep = currentStep;
            return success ? Result.Ok(obj) : Result.Fail<T>(new Failure("oops"));
        }

        public async Task<IResult> GetResultAsync(int currentStep, bool success = true)
        {
            if (LastStep != currentStep - 1)
            {
                throw new InvalidOperationException();
            }

            await Task.Delay(10);
            LastStep = currentStep;
            return success ? Result.Ok() : Result.Fail(new Failure("oops"));
        }

        public async Task<IResult<T>> GetResultAsync<T>(T obj, int currentStep, bool success = true)
        {
            if (LastStep != currentStep - 1)
            {
                throw new InvalidOperationException();
            }

            await Task.Delay(10);
            LastStep = currentStep;
            return success ? Result.Ok(obj) : Result.Fail<T>(new Failure("oops"));
        }
    }
}
