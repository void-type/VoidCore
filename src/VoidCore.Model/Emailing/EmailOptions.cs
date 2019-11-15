using System.Collections.Generic;

namespace VoidCore.Model.Emailing
{
    public sealed class EmailOptions
    {
        internal EmailOptions(string subject, IReadOnlyList<string> bodyLines, IReadOnlyList<string> recipients)
        {
            Subject = subject;
            BodyLines = bodyLines;
            Recipients = recipients;
        }

        public string Subject { get; }
        public IReadOnlyList<string> BodyLines { get; }
        public IReadOnlyList<string> Recipients { get; }
    }
}
