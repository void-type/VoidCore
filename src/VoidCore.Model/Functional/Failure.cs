namespace VoidCore.Model.Functional;

/// <inheritdoc/>
public class Failure : IFailure
{
    /// <summary>
    /// Construct a new Failure.
    /// </summary>
    /// <param name="errorMessage">UI friendly error message</param>
    /// <param name="uiHandle">The entity property name that is in error. Can be mapped to a field on the view</param>
    /// <param name="code">A code name or identifier for the error. Can be used for discrimination.</param>
    public Failure(string errorMessage, string? uiHandle = null, string? code = null)
    {
        Message = errorMessage;
        UiHandle = uiHandle;
        Code = code;
    }

    /// <inheritdoc/>
    public string Message { get; }

    /// <inheritdoc/>
    public string? UiHandle { get; }

    /// <inheritdoc/>
    public string? Code { get; }
}
