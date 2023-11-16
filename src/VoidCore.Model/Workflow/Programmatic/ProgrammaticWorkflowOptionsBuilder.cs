using System;
using System.Collections.Generic;

namespace VoidCore.Model.Workflow.Programmatic;

/// <summary>
/// A fluent builder for configuring allowed workflow transitions.
/// </summary>
public class ProgrammaticWorkflowOptionsBuilder<TState, TCommand>
{
    internal ProgrammaticWorkflowOptionsBuilder() { }

    private readonly HashSet<ProgrammaticWorkflowTransition<TState, TCommand>> _transitions = [];

    /// <summary>
    /// Add an allowed transition between states.
    /// </summary>
    /// <param name="currentState">The starting state</param>
    /// <param name="command">The requested command</param>
    /// <param name="endingState">The resultant state</param>
    /// <exception cref="InvalidOperationException">Throws when duplicate transitions are added.</exception>
    public ProgrammaticWorkflowOptionsBuilder<TState, TCommand> AddTransition(TState currentState, TCommand command, TState endingState)
    {
        var newTransition = new ProgrammaticWorkflowTransition<TState, TCommand>(currentState, command, endingState);

        if (!_transitions.Add(newTransition))
        {
            throw new InvalidOperationException($"Cannot add a duplicate transition of {currentState} => {command}.");
        }

        return this;
    }

    internal IReadOnlyCollection<ProgrammaticWorkflowTransition<TState, TCommand>> Build()
    {
        return _transitions;
    }
}
