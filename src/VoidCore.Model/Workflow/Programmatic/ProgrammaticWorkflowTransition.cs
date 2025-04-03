using VoidCore.Model.Functional;
using VoidCore.Model.Guards;

namespace VoidCore.Model.Workflow.Programmatic;

/// <summary>
/// A pairing of the current workflow state and a command.
/// </summary>
internal class ProgrammaticWorkflowTransition<TState, TCommand> : ValueObject
{
    internal ProgrammaticWorkflowTransition(TState currentState, TCommand command, TState nextState)
    {
        CurrentState = currentState.EnsureNotNull();
        Command = command.EnsureNotNull();
        NextState = nextState.EnsureNotNull();
    }

    internal TState CurrentState { get; }
    internal TCommand Command { get; }
    internal TState NextState { get; }

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CurrentState!;
        yield return Command!;
    }
}
