using System;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Functional;

namespace VoidCore.Model.Workflow.Programmatic;

/// <summary>
/// A stateless service to track allowed state transitions in a workflow.
/// This variant uses classes for commands and state and isn't abstract. This allows constructing workflows from data at runtime.
/// </summary>
public class ProgrammaticWorkflow<TState, TCommand>
        where TState : class
        where TCommand : class
{
    private readonly IReadOnlyCollection<ProgrammaticWorkflowTransition<TState, TCommand>> _transitions;

    /// <summary>
    /// Constructor with an options builder action.
    /// </summary>
    /// <param name="builderAction">An action to configure transitions</param>
    public ProgrammaticWorkflow(Action<ProgrammaticWorkflowOptionsBuilder<TState, TCommand>> builderAction)
    {
        var builder = new ProgrammaticWorkflowOptionsBuilder<TState, TCommand>();
        builderAction(builder);
        _transitions = builder.Build();
    }

    /// <summary>
    /// Get the next state in the workflow.
    /// </summary>
    /// <param name="currentState">The current state of the flow</param>
    /// <param name="command">The command used on the current state</param>
    public IResult<TState> GetNext(TState currentState, TCommand command)
    {
        var maybeValidTransition = Maybe.From(_transitions
            .FirstOrDefault(t => t.CurrentState.Equals(currentState) && t.Command.Equals(command)));

        return maybeValidTransition
            .ToResult(new Failure($"Invalid transition: {currentState} => {command}."))
            .Select(t => t.NextState);
    }

    /// <summary>
    /// Get the available commands given the current state of the workflow.
    /// </summary>
    /// <param name="currentState">The current state of the flow</param>
    public IEnumerable<TCommand> GetAvailableCommands(TState currentState)
    {
        return _transitions
            .Where(t => t.CurrentState.Equals(currentState))
            .Select(t => t.Command);
    }
}
