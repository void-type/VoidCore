using System.Collections.Generic;
using VoidCore.Domain.Guards;

namespace VoidCore.Model.Emailing
{
    public sealed class EmailOptionsBuilder
    {
        private readonly IAppVariables _variables;

        public EmailOptionsBuilder(IAppVariables variables)
        {
            _variables = variables;
        }

        private string _subject = string.Empty;
        private readonly List<string> _bodyLines = new List<string>();
        private readonly List<string> _recipients = new List<string>();

        public void SetSubject(string subject, bool includeAppNamePrefix = false)
        {
            subject.EnsureNotNullOrEmpty(nameof(subject));

            _subject = includeAppNamePrefix ?
                $"{_variables.AppName}: {subject}" :
                subject;
        }

        public void AddLine(string line = "")
        {
            _bodyLines.Add(line);
        }

        public void AddRecipient(string recipient)
        {
            recipient.EnsureNotNullOrEmpty(nameof(recipient));
            _recipients.Add(recipient);
        }

        public EmailOptions Build()
        {
            return new EmailOptions(_subject, _bodyLines, _recipients);
        }
    }
}
