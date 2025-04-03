namespace VoidCore.Test.Model.Functional;

/// <summary>
/// This class gives something for our functional extensions to act on.
/// This class also ensures that each prior step is performed in order (or awaited) by keeping a lastStep count.
/// </summary>
internal class TestPerformerService
{
    private int _lastStep;

    public static string Start => "Hello World";

    public void Do(int currentStep)
    {
        CheckStep(currentStep);
        _lastStep = currentStep;
    }

    public async Task DoAsync(int currentStep)
    {
        CheckStep(currentStep);
        await Task.Delay(10);
        _lastStep = currentStep;
    }

    public void Reset()
    {
        _lastStep = default;
    }

    private void CheckStep(int currentStep)
    {
        if (_lastStep != currentStep - 1)
        {
            throw new InvalidOperationException("Concurrency error.");
        }
    }
}
