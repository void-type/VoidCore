using System.Collections.Generic;
using VoidCore.Domain.Guards;

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
        public Email(string subject, string message, IEnumerable<string> recipients)
        {
            Subject = subject.EnsureNotNullOrEmpty(nameof(subject));
            Message = message.EnsureNotNullOrEmpty(nameof(message));
            Recipients = recipients.EnsureNotNull(nameof(recipients));
        }

        /// <summary>
        /// Construct a new email
        /// </summary>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="message">The message content of the email</param>
        /// <param name="recipients">The recipients of the email.</param>
        public Email(string subject, string message, params string[] recipients)
        {
            Subject = subject.EnsureNotNullOrEmpty(nameof(subject));
            Message = message.EnsureNotNullOrEmpty(nameof(message));
            Recipients = recipients.EnsureNotNull(nameof(recipients));
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
