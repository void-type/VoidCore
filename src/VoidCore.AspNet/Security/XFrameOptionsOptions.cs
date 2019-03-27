namespace VoidCore.AspNet.Security
{
    /// <summary>
    /// Options for configuring the X-Frame-Options header.
    /// </summary>
    public sealed class XFrameOptionsOptions
    {
        /// <summary>
        /// The value of the header.
        /// </summary>
        public string Value { get; }

        internal XFrameOptionsOptions(string option)
        {
            Value = option;
        }
    }
}
