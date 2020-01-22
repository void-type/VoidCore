using System;
using System.Collections.Generic;

namespace VoidCore.Domain.Workflow
{
    /// <summary>
    /// A fluent builder for configuring allowed workflow transitions.
    /// </summary>
    public class WorkflowOptionsBuilder<TState, TCommand>
        where TState : Enum
        where TCommand : Enum
    {
        internal WorkflowOptionsBuilder() { }

        private readonly Dictionary<Transition<TState, TCommand>, TState> _transitions = new Dictionary<Transition<TState, TCommand>, TState>();

        /// <summary>
        /// Add an allowed transition between states.
        /// </summary>
        /// <param name="startingState">The starting state</param>
        /// <param name="command">The requested command</param>
        /// <param name="endingState">The resultant state</param>
        public WorkflowOptionsBuilder<TState, TCommand> AddTransition(TState startingState, TCommand command, TState endingState)
        {
            _transitions.Add(new Transition<TState, TCommand>(startingState, command), endingState);
            return this;
        }

        internal Dictionary<Transition<TState, TCommand>, TState> Build()
        {
            return _transitions;
        }
    }
}
