using System.Threading;
using System.Threading.Tasks;

namespace VoidCore.Model.Emailing
{
    /// <summary>
    /// A service for sending emails.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Send the email to its recipients.
        /// </summary>
        /// <param name="email">The email to send</param>
        /// <param name="cancellationToken">The cancellation token to cancel the task</param>
        Task SendEmail(Email email, CancellationToken cancellationToken);
    }
}
