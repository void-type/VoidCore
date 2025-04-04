﻿using VoidCore.Model.Guards;

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
        Subject = subject.EnsureNotNull();
        Message = message.EnsureNotNull();
        Recipients = recipients.EnsureNotNull();
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
}
