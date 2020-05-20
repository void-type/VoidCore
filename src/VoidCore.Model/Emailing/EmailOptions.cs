using System.Collections.Generic;

namespace VoidCore.Model.Emailing
{
    /// <summary>
    /// Options for building an email.
    /// </summary>
    public sealed class EmailOptions
    {
        internal EmailOptions(string subject, IReadOnlyList<string> messageLines, IReadOnlyList<string> recipients)
        {
            Subject = subject;
            MessageLines = messageLines;
            Recipients = recipients;
        }

        /// <summary>
        /// The subject of the email.
        /// </summary>
        internal string Subject { get; }

        /// <summary>
        /// The lines of the body of the email.
        /// </summary>
        internal IReadOnlyList<string> MessageLines { get; }

        /// <summary>
        /// The addresses that the email will be sent to.
        /// </summary>
        internal IReadOnlyList<string> Recipients { get; }
    }
}
