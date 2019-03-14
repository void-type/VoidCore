using System;
using System.Collections.Generic;

namespace VoidCore.Model.Emailing
{
    /// <summary>
    /// A simple, text-based email that can be sent via an emailing service.
    /// </summary>
    public class Email
    {
        /// <summary>
        /// Construct a new email
        /// </summary>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="message">The message content of the email</param>
        /// <param name="recipients">The recipients of the email.</param>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException if any parameters are null.</exception>
        public Email(string subject, string message, IEnumerable<string> recipients)
        {
            Subject = subject ??
                throw new ArgumentNullException(nameof(subject), "Parameter cannot be null.");
            Message = message ??
                throw new ArgumentNullException(nameof(message), "Parameter cannot be null.");
            Recipients = recipients ??
                throw new ArgumentNullException(nameof(recipients), "Parameter cannot be null.");
        }

        /// <summary>
        /// The message content of the email.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// A list of recipients to send the email to.
        /// </summary>
        public IEnumerable<string> Recipients { get; }

        /// <summary>
        /// The subject line of the email.
        /// </summary>
        public string Subject { get; }
    }
}
