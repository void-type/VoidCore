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
            CheckStep(currentStep);
            _lastStep = currentStep;
        }

        public async Task DoAsync(string a, int currentStep)
        {
            CheckStep(currentStep);
            await Task.Delay(10);
            _lastStep = currentStep;
        }

        public void Go(int currentStep)
        {
            CheckStep(currentStep);
            _lastStep = currentStep;
        }

        public async Task GoAsync(int currentStep)
        {
            CheckStep(currentStep);
            await Task.Delay(10);
            _lastStep = currentStep;
        }

        private void CheckStep(int currentStep)
        {
            if (_lastStep != currentStep - 1)
            {
                throw new InvalidOperationException("Concurrency error.");
            }
        }
    }
}
