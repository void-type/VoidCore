using System;

namespace VoidCore.AspNet.Configuration
{
    /// <summary>
    /// Helpers to assist in configuring the application.
    /// </summary>
    public static class ConfigHelpers
    {
        /// <summary>
        /// Strips an ending from a class type name. This is useful for convention-based naming to replace hardcoded strings.
        /// Ex: AuthorizationSettings, "settings" => "Authorization"
        /// Ex: Authorization, "settings" => "Authorization"
        /// Ex: AuthorizationSettings, null => "AuthorizationSettings"
        /// </summary>
        /// <param name="type">The type to get the name from</param>
        /// <param name="ending">The ending to remove from the class type name</param>
        /// <returns>Type name with the ending removed</returns>
        public static string StripEndingFromType(Type type, string ending)
        {
            var rawName = type.Name;
            var nameEnd = rawName.Length;

            if (ending == null)
            {
                return rawName.Substring(0, nameEnd);
            }

            var lastIndexOfEnding = rawName.ToLower().LastIndexOf(ending, StringComparison.Ordinal);

            if (lastIndexOfEnding > -1)
            {
                nameEnd = lastIndexOfEnding;
            }

            return rawName.Substring(0, nameEnd);
        }
    }
}
