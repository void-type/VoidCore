namespace VoidCore.AspNet.Security
{
    /// <summary>
    /// A header for adding X-Frame-Options to a webpage.
    /// </summary>
    public class XFrameOptionsHeader
    {
        /// <summary>
        /// Construct a new XFrameOptionsHeader
        /// </summary>
        /// <param name="options">The XFrameOptionsOptions to configure the header with</param>
        public XFrameOptionsHeader(XFrameOptionsOptions options)
        {
            Key = "X-Frame-Options";
            Value = options.Value;
        }

        /// <summary>
        /// The header key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// The header value.
        /// </summary>
        public string Value { get; }
    }
}
