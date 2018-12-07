using VoidCore.Domain;

namespace VoidCore.Model.Emailing
{
    /// <summary>
    /// A service for sending emails.
    /// </summary>
    public interface IEmailerService
    {
        /// <summary>
        /// Send the email to its recipients.
        /// </summary>
        /// <param name="email">The email to send</param>
        /// <returns>A result describing the success of sending the email</returns>
        Result SendEmail(Email email);
    }
}
