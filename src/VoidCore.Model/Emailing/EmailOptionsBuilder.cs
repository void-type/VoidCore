using System.Collections.Generic;
using VoidCore.Domain.Guards;

namespace VoidCore.Model.Emailing
{
    /// <summary>
    /// A builder to configure options for an email.
    /// </summary>
    public sealed class EmailOptionsBuilder
    {
        internal EmailOptionsBuilder() { }

        private string _subject = string.Empty;
        private readonly List<string> _messageLines = new List<string>();
        private readonly List<string> _recipients = new List<string>();

        /// <summary>
        /// Set the subject of the email. Will override any previously set subjects.
        /// </summary>
        /// <param name="subject">The email subject</param>
        public void SetSubject(string subject)
        {
            _subject = subject.EnsureNotNullOrEmpty(nameof(subject));
        }

        /// <summary>
        /// Add a line to the body of the email. New line delimiters are added automatically upon building the email.
        /// </summary>
        /// <param name="line">The line to add to the email.</param>
        public void AddLine(string line = "")
        {
            _messageLines.Add(line);
        }

        /// <summary>
        /// Add a recipient email address to the email.
        /// </summary>
        /// <param name="recipient">The email address of the recipient</param>
        public void AddRecipient(string recipient)
        {
            recipient.EnsureNotNullOrEmpty(nameof(recipient));
            _recipients.Add(recipient);
        }

        internal EmailOptions Build()
        {
            return new EmailOptions(_subject, _messageLines, _recipients);
        }
    }
}
