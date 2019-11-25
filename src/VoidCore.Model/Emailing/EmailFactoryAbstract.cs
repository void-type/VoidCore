using System;
using VoidCore.Domain;

namespace VoidCore.Model.Emailing
{
    /// <summary>
    /// A base class for building emails using a builder action.
    /// The implementation of the class controls how the parts of the email are constructed.
    /// </summary>
    public abstract class EmailFactoryAbstract : IEmailFactory
    {
        /// <inheritdoc/>
        public Email Create(Action<EmailOptionsBuilder> configure)
        {
            return new EmailOptionsBuilder()
                .Tee(configure)
                .Map(builder => builder.Build())
                .Map(CreateEmail);
        }

        /// <summary>
        /// Override this method with the details of constructing the email from the options configured.
        /// </summary>
        /// <param name="options">The options configured by the Build method caller</param>
        protected abstract Email CreateEmail(EmailOptions options);
    }
}
