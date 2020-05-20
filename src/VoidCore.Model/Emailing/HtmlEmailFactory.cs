namespace VoidCore.Model.Emailing
{
    /// <summary>
    /// Build an HTML-based email. Note that the content of the email will be parsed as raw html.
    /// Interpolated strings are NOT escaped. Please be cautious of XSS and CSRF vulnerabilities when building the email.
    /// </summary>
    public sealed class HtmlEmailFactory : EmailFactoryAbstract
    {
        /// <inheritdoc/>
        protected override Email CreateEmail(EmailOptions options)
        {
            var content = "<html><body>" + string.Join("<br>", options.MessageLines) + "</body></html>";

            return new Email(options.Subject, content, options.Recipients);
        }
    }
}
