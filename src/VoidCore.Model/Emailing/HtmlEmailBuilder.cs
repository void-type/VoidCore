using System;
using VoidCore.Domain;

namespace VoidCore.Model.Emailing
{
    public sealed class HtmlEmailBuilder : IEmailBuilder
    {
        private readonly IAppVariables _variables;

        public HtmlEmailBuilder(IAppVariables variables)
        {
            _variables = variables;
        }

        public Email Build(Action<EmailOptionsBuilder> configure)
        {
            var options = new EmailOptionsBuilder(_variables)
                .Tee(builder => configure(builder))
                .Map(builder => builder.Build());

            var content = "<html><body>" + string.Join("<br>", options.BodyLines) + "</body></html>";

            return new Email(options.Subject, content, options.Recipients);
        }
    }
}

