using VoidCore.Model.Guards;

namespace VoidCore.AspNet.Security
{
    /// <summary>
    /// Builds the options to configure the X-Frame-Options header.
    /// </summary>
    public sealed class XFrameOptionsOptionsBuilder
    {
        private const string DenyValue = "deny";
        private string _option = DenyValue;

        /// <summary>
        /// Construct a new XFrameOptionsOptionsBuilder. Default option is denied.
        /// </summary>
        public XFrameOptionsOptionsBuilder() { }

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
            _option = "sameorigin";
        }

        /// <summary>
        /// Allow this page to be shown on a page from the specified origin URI.
        /// </summary>
        /// <param name="originUri">The origin URI of a page that can frame this page.</param>
        public void AllowFrom(string originUri)
        {
            originUri.EnsureNotNullOrEmpty(nameof(originUri));

            _option = $"allow-from {originUri}";
        }

        /// <summary>
        /// Build the XFrameOptionsOptions as configured by this builder.
        /// </summary>
        /// <returns>A new XFrameOptionsOptions</returns>
        public XFrameOptionsOptions Build()
        {
            return new XFrameOptionsOptions(_option);
        }
    }
}
