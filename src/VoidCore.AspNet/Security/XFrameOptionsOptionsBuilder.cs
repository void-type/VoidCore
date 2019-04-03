namespace VoidCore.AspNet.Security
{
    /// <summary>
    /// Builds the options to configure the X-Frame-Options header.
    /// </summary>
    public sealed class XFrameOptionsOptionsBuilder
    {
        private const string DenyValue = "deny";
        private const string SameOriginValue = "sameorigin";
        private const string AllowFromValue = "allow-from";
        private string _option = DenyValue;

        internal XFrameOptionsOptionsBuilder() { }

        /// <summary>
        /// Deny this page from being shown in a frame.
        /// </summary>
        public void Deny()
        {
            _option = DenyValue;
        }

        /// <summary>
        /// Allow this page to be shown in frames from the same origin server.
        /// </summary>
        public void SameOrigin()
        {
            _option = SameOriginValue;
        }

        /// <summary>
        /// Allow this page to be shown on a page from the specified origin URI.
        /// </summary>
        /// <param name="originUri">The origin URI of a page that can frame this page.</param>
        public void AllowFrom(string originUri)
        {
            _option = $"{AllowFromValue} {originUri}";
        }

        internal XFrameOptionsOptions Build()
        {
            return new XFrameOptionsOptions(_option);
        }
    }
}
