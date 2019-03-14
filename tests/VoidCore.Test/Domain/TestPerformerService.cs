using System;
using System.Threading.Tasks;

namespace VoidCore.Test.Domain
{
    internal class TestPerformerService
    {
        private int _lastStep;

        public static string Start => "Hello World";

        public void Do(string a, int currentStep)
        {
            if (_lastStep != currentStep - 1)
            {
                throw new InvalidOperationException();
            }

            _lastStep = currentStep;
        }

        public async Task DoAsync(string a, int currentStep)
        {
            if (_lastStep != currentStep - 1)
            {
                throw new InvalidOperationException();
            }

            await Task.Delay(10);
            _lastStep = currentStep;
        }

        public void Go(int currentStep)
        {
            if (_lastStep != currentStep - 1)
            {
                throw new InvalidOperationException();
            }

            _lastStep = currentStep;
        }

        public async Task GoAsync(int currentStep)
        {
            if (_lastStep != currentStep - 1)
            {
                throw new InvalidOperationException();
            }

            await Task.Delay(10);
            _lastStep = currentStep;
        }
    }
}
