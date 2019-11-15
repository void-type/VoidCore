using System;
using VoidCore.Domain;

namespace VoidCore.Model.Emailing
{
    public sealed class TextEmailBuilder : IEmailBuilder
    {
        private readonly IAppVariables _variables;

        public TextEmailBuilder(IAppVariables variables)
        {
            _variables = variables;
        }

        public Email Build(Action<EmailOptionsBuilder> configure)
        {
            var options = new EmailOptionsBuilder(_variables)
                .Tee(builder => configure(builder))
                .Map(builder => builder.Build());

            var content = string.Join("\r\n", options.BodyLines);

            return new Email(options.Subject, content, options.Recipients);
        }
    }
}

