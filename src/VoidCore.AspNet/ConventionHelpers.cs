using System;

namespace VoidCore.AspNet
{
    /// <summary>
    /// Helpers to assist in configuring the application through class naming conventions.
    /// </summary>
    public static class ConventionHelpers
    {
        /// <summary>
        /// Strips an ending from a class type name. This is useful for convention-based naming to replace hardcoded strings.
        /// Ex: AuthorizationSettings, "settings" becomes "Authorization"
        /// Ex: Authorization, "settings" becomes "Authorization"
        /// Ex: AuthorizationSettings, null becomes "AuthorizationSettings"
        /// </summary>
        /// <param name="type">The type to get the name from</param>
        /// <param name="ending">The ending to remove from the class type name</param>
        public static string GetTypeNameWithoutEnding(this Type type, string ending)
        {
            var rawName = type.Name.Split('`')[0];

            if (string.IsNullOrWhiteSpace(ending))
            {
                return rawName;
            }

            var lastIndexOfEnding = rawName.ToLower().LastIndexOf(ending, StringComparison.OrdinalIgnoreCase);

            if (lastIndexOfEnding < 0)
            {
                return rawName;
            }

            return rawName.Substring(0, lastIndexOfEnding);
        }

        /// <summary>
        /// Get the name of class as it would appear in source code.
        /// </summary>
        /// <param name="type">The type to get the name of.</param>
        public static string GetFriendlyTypeName(this Type type)
        {
            var name = type.Name;

            if (!type.IsGenericType)
            {
                return name;
            }

            name = name.Split('`')[0] + "<";

            var genericArguments = type.GetGenericArguments();

            var genericArgumentsNames = new string[genericArguments.Length];

            for (var i = genericArguments.Length - 1; i >= 0; i--)
            {
                genericArgumentsNames[i] = GetFriendlyTypeName(genericArguments[i]);
            }

            name += string.Join(", ", genericArgumentsNames);

            name += ">";

            return name;
        }
    }
}
