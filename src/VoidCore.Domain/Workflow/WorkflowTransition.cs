using System;
using System.Collections.Generic;

namespace VoidCore.Domain.Workflow
{
    /// <summary>
    /// A pairing of the current workflow state and a command.
    /// </summary>
    internal class WorkflowTransition<TState, TCommand> : ValueObject
            where TState : Enum
            where TCommand : Enum
    {
        internal WorkflowTransition(TState currentState, TCommand command, TState nextState)
        {
            CurrentState = currentState;
            Command = command;
            NextState = nextState;
        }

        internal TState CurrentState { get; }
        internal TCommand Command { get; }
        internal TState NextState { get; }

        /// <inheritdoc />
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CurrentState;
            yield return Command;
        }
    }
}
