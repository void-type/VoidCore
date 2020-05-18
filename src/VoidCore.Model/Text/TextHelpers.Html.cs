namespace VoidCore.Model.Text
{
    public static partial class TextHelpers
    {
        /// <summary>
        /// Make an anchor tag string.
        /// </summary>
        /// <param name="caption">The text shown to the user</param>
        /// <param name="urlSegments">A series of url segments to be joined with "/"</param>
        /// <returns></returns>
        public static string Link(string caption, params string[] urlSegments)
        {
            return $"<a href=\"{string.Join("/", urlSegments)}\">{caption}</a>";
        }
    }
}
