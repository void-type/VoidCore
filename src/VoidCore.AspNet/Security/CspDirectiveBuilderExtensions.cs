namespace VoidCore.AspNet.Security
{
    /// <summary>
    /// Extensions to wrap directive builders with methods that ease building directives to the specification.
    /// </summary>
    public static class CspDirectiveBuilderExtensions
    {
        /// <summary>
        /// Enhance a CspDirectiveBuilder with methods that support "src" directives.
        /// </summary>
        /// <param name="cspDirectiveBuilder"></param>
        /// <returns></returns>
        public static CspSourceDirectiveBuilder ForSources(this CspDirectiveBuilder cspDirectiveBuilder)
        {
            return new CspSourceDirectiveBuilder(cspDirectiveBuilder);
        }
    }
}
