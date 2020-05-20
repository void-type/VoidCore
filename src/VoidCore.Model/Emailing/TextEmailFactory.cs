namespace VoidCore.Model.Emailing
{
    /// <summary>
    /// Build a text-only email.
    /// </summary>
    public sealed class TextEmailFactory : EmailFactoryAbstract
    {
        /// <inheritdoc/>
        protected override Email CreateEmail(EmailOptions options)
        {
            var content = string.Join("\r\n", options.MessageLines);

            return new Email(options.Subject, content, options.Recipients);
        }
    }
}
