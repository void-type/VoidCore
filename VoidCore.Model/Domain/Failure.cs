﻿namespace VoidCore.Model.Domain
{
    /// <inheritdoc/>
    public class Failure : IFailure
    {
        /// <inheritdoc/>
        public string Message { get; }

        /// <inheritdoc/>
        public string UiHandle { get; }

        /// <summary>
        /// Construct a new Failure.
        /// </summary>
        /// <param name="errorMessage">UI friendly error message</param>
        /// <param name="uiHandle">The entity property name that is in error. Can be mapped to a field on the view</param>
        public Failure(string errorMessage, string uiHandle = null)
        {
            Message = errorMessage;
            UiHandle = uiHandle;
        }
    }
}