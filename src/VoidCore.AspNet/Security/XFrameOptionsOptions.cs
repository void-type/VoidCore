namespace VoidCore.AspNet.Security;

/// <summary>
/// Options for configuring the X-Frame-Options header.
/// </summary>
public sealed class XFrameOptionsOptions
{
    internal XFrameOptionsOptions(string option)
    {
        Value = option;
    }

    /// <summary>
    /// The value of the header.
    /// </summary>
    internal string Value { get; }
}
