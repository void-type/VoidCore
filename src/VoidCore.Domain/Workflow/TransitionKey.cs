using System;
using System.Collections.Generic;

namespace VoidCore.Domain.Workflow
{
    /// <summary>
    /// A pairing of the current workflow state and a command.
    /// </summary>
    internal class TransitionKey<TState, TCommand> : ValueObject
            where TState : Enum
            where TCommand : Enum
    {
        private readonly TState _currentState;
        private readonly TCommand _command;

        internal TransitionKey(TState currentState, TCommand command)
        {
            _currentState = currentState;
            _command = command;
        }

        /// <inheritdoc />
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _currentState;
            yield return _command;
        }
    }
}
