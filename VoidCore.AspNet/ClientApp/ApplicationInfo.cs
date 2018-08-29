namespace VoidCore.AspNet.ClientApp
{
    /// <inheritdoc/>
    public class ApplicationInfo : IApplicationInfo
    {
        /// <inheritdoc/>
        public string ApplicationName { get; set; }

        /// <inheritdoc/>
        public string AntiforgeryToken { get; set; }

        /// <inheritdoc/>
        public string AntiforgeryTokenHeaderName { get; set; }

        /// <inheritdoc/>
        public string UserName { get; set; }
    }
}
