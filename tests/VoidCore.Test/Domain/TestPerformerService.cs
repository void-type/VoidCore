using System;
using System.Threading.Tasks;

namespace VoidCore.Test.Domain
{
    internal class TestPerformerService
    {
        public int LastStep { get; private set; }
        public string Start => "Hello World";

        public void Do(string a, int currentStep)
        {
            if (LastStep != currentStep - 1)
            {
                throw new InvalidOperationException();
            }

            LastStep = currentStep;
        }

        public async Task DoAsync(string a, int currentStep)
        {
            if (LastStep != currentStep - 1)
            {
                throw new InvalidOperationException();
            }

            await Task.Delay(10);
            LastStep = currentStep;
        }

        public void Go(int currentStep)
        {
            if (LastStep != currentStep - 1)
            {
                throw new InvalidOperationException();
            }

            LastStep = currentStep;
        }

        public async Task GoAsync(int currentStep)
        {
            if (LastStep != currentStep - 1)
            {
                throw new InvalidOperationException();
            }

            await Task.Delay(10);
            LastStep = currentStep;
        }
    }
}
