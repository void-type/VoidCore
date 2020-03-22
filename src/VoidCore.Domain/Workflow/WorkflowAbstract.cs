using System;
using System.Collections.Generic;

namespace VoidCore.Domain.Workflow
{
    /// <summary>
    /// A stateless service to track allowed state transitions in a workflow.
    /// </summary>
    public abstract class WorkflowAbstract<TState, TCommand>
            where TState : Enum
            where TCommand : Enum
    {
        private readonly Dictionary<TransitionKey<TState, TCommand>, TState> _transitions = new Dictionary<TransitionKey<TState, TCommand>, TState>();

        /// <summary>
        /// Constructor with an options builder action.
        /// </summary>
        /// <param name="builderAction">An action to configure transitions</param>
        protected WorkflowAbstract(Action<WorkflowOptionsBuilder<TState, TCommand>> builderAction)
        {
            var builder = new WorkflowOptionsBuilder<TState, TCommand>();
            builderAction.Invoke(builder);
            _transitions = builder.Build();
        }

        /// <summary>
        /// Get the next state in the workflow.
        /// </summary>
        /// <param name="currentState">The current state of the flow</param>
        /// <param name="command">The command used on the current state</param>
        public IResult<TState> GetNext(TState currentState, TCommand command)
        {
            var requestedTransition = new TransitionKey<TState, TCommand>(currentState, command);

            if (!_transitions.TryGetValue(requestedTransition, out TState nextState))
            {
                return Result.Fail<TState>(new Failure($"Invalid transition: {currentState} => {command}"));
            }

            return Result.Ok(nextState);
        }
    }
}
