using System.Linq;
using VoidCore.Model.Action.Responder;
using VoidCore.Model.Action.Responses.ItemSet;
using VoidCore.Model.Validation;

namespace VoidCore.Model.Action.Steps
{
    /// <summary>
    /// Validate an entity.
    /// </summary>
    /// <typeparam name="TValidatable"></typeparam>
    public class Validate<TValidatable> : IActionStep
    {
        /// <summary>
        /// Construct a new action.
        /// </summary>
        /// <param name="validator">The validator to use against the entity</param>
        /// <param name="validatableEntity">The entity to validate</param>
        /// <param name="logMessage">Additional logging information</param>
        public Validate(IValidator<TValidatable> validator, TValidatable validatableEntity, string logMessage = null)
        {
            _validator = validator;
            _validatableEntity = validatableEntity;
            _logMessage = logMessage;
        }

        /// <inheritdoc/>
        public void Perform(IActionResponder respond)
        {
            var validationErrors = _validator.Validate(_validatableEntity).ToItemSet();

            if (validationErrors.Items.Any())
            {
                var fullLogText = validationErrors.GetLogText().Concat(new [] { _logMessage }).ToArray();
                respond.WithWarning(validationErrors, fullLogText);
            }
        }

        private readonly string _logMessage;
        private readonly TValidatable _validatableEntity;
        private readonly IValidator<TValidatable> _validator;
    }
}
