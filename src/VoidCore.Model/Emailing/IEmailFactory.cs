namespace VoidCore.Model.Emailing;

/// <summary>
/// Builds emails via an action to configure options.
/// </summary>
public interface IEmailFactory
{
    /// <summary>
    /// Configure and build the email.
    /// </summary>
    /// <param name="configure">An action to configure the email using an EmailOptionsBuilder</param>
    Email Create(Action<EmailOptionsBuilder> configure);
}
