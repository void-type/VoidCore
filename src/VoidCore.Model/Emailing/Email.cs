using VoidCore.Model.Guards;

namespace VoidCore.Model.Emailing;

/// <summary>
/// An email that can be sent via an emailing service.
/// </summary>
public class Email
{
    /// <summary>
    /// Construct a new email.
    /// </summary>
    /// <param name="subject">The subject line of the email</param>
    /// <param name="message">The message content of the email</param>
    /// <param name="recipients">The recipients of the email.</param>
    public Email(string subject, string message, IEnumerable<string> recipients)
    {
        Subject = EnsureSingleLine(subject.EnsureNotNull(), nameof(subject));
        Message = message.EnsureNotNull();

        // Copy so later changes to the caller's collection can't change who gets the email.
        Recipients = recipients.EnsureNotNull()
            .Select(recipient => EnsureRecipient(recipient, nameof(recipients)))
            .ToArray();
    }

    /// <summary>
    /// Construct a new email.
    /// </summary>
    /// <param name="subject">The subject line of the email</param>
    /// <param name="message">The message content of the email</param>
    /// <param name="recipients">The recipients of the email.</param>
    public Email(string subject, string message, params string[] recipients) : this(subject, message, recipients.AsEnumerable()) { }

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

    internal static string EnsureRecipient(string? recipient, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(recipient))
        {
            throw new ArgumentException("The recipient can't be blank.", parameterName);
        }

        return EnsureSingleLine(recipient, parameterName);
    }

    // A line break in a header value, like the subject or an address, can inject extra headers into the message.
    internal static string EnsureSingleLine(string value, string parameterName)
    {
        if (value.AsSpan().ContainsAny('\r', '\n'))
        {
            throw new ArgumentException("The value can't contain line breaks.", parameterName);
        }

        return value;
    }
}
