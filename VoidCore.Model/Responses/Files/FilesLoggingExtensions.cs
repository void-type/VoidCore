namespace VoidCore.Model.Responses.Files
{
    /// <summary>
    /// Helpers for pulling logging text from the default responses.
    /// </summary>
    public static class FilesLoggingExtensions
    {
        /// <summary>
        /// Pulls default logging text from this object.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string[] GetLogText(this ISimpleFile file)
        {
            return new []
            {
                $"FileName: {file.Name}"
            };
        }
    }
}
