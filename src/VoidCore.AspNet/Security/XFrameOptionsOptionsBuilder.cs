namespace VoidCore.AspNet.Security
{
    /// <summary>
    /// Builds the options to configure the X-Frame-Options header.
    /// </summary>
    public sealed class XFrameOptionsOptionsBuilder
    {
        private const string _deny = "deny";
        private const string _sameOrigin = "sameorigin";
        private const string _allowFrom = "allow-from";
        private string _option = _deny;

        internal XFrameOptionsOptionsBuilder() { }

        /// <summary>
        /// Deny this page from being shown in a frame.
        /// </summary>
        public void Deny()
        {
            _option = _deny;
        }

        /// <summary>
        /// Allow this page to be shown in frames from the same origin server.
        /// </summary>
        public void SameOrigin()
        {
            _option = _sameOrigin;
        }

        /// <summary>
        /// Allow this page to be shown on a page from the specified origin URI.
        /// </summary>
        /// <param name="originUri">The origin URI of a page that can frame this page.</param>
        public void AllowFrom(string originUri)
        {
            _option = $"{_allowFrom} {originUri}";
        }

        internal XFrameOptionsOptions Build()
        {
            return new XFrameOptionsOptions(_option);
        }
    }
}
