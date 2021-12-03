using System;
using System.Collections.Generic;

namespace VoidCore.Model.Workflow;

/// <summary>
/// A fluent builder for configuring allowed workflow transitions.
/// </summary>
public class WorkflowOptionsBuilder<TState, TCommand>
    where TState : Enum
    where TCommand : Enum
{
    internal WorkflowOptionsBuilder() { }

    private readonly HashSet<WorkflowTransition<TState, TCommand>> _transitions = new();

    /// <summary>
    /// Add an allowed transition between states.
    /// </summary>
    /// <param name="currentState">The starting state</param>
    /// <param name="command">The requested command</param>
    /// <param name="endingState">The resultant state</param>
    /// <exception cref="InvalidOperationException">Throws when duplicate transitions are added.</exception>
    public WorkflowOptionsBuilder<TState, TCommand> AddTransition(TState currentState, TCommand command, TState endingState)
    {
        var newTransition = new WorkflowTransition<TState, TCommand>(currentState, command, endingState);

        if (!_transitions.Add(newTransition))
        {
            throw new InvalidOperationException($"Cannot add a duplicate transition of {currentState} => {command}.");
        }

        return this;
    }

    internal IReadOnlyCollection<WorkflowTransition<TState, TCommand>> Build()
    {
        return _transitions;
    }
}
