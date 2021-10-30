using System;
using System.Linq;
using VoidCore.Model.Functional;

namespace VoidCore.Model.Text
{
    /// <summary>
    /// Helpers to assist in configuring the application through class naming conventions.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Strips an ending from a class type name. This is useful for convention-based naming to replace hardcoded strings.
        /// Ex: (AuthorizationSettings, "settings") becomes "Authorization"
        /// Ex: (Authorization, "settings") becomes "Authorization"
        /// Ex: (AuthorizationSettings, null) becomes "AuthorizationSettings"
        /// </summary>
        /// <param name="type">The type to get the name from</param>
        /// <param name="ending">The ending to remove from the class type name</param>
        public static string GetTypeNameWithoutEnding(this Type type, string? ending)
        {
            var rawName = type.Name.Split('`')[0];

            if (string.IsNullOrWhiteSpace(ending))
            {
                return rawName;
            }

            var lastIndexOfEnding = rawName.LastIndexOf(ending, StringComparison.OrdinalIgnoreCase);

            if (lastIndexOfEnding < 0)
            {
                return rawName;
            }

            return rawName[..lastIndexOfEnding];
        }

        /// <summary>
        /// Get the name of class as it would appear in source code.
        /// </summary>
        /// <param name="type">The type to get the name of.</param>
        public static string GetFriendlyTypeName(this Type type)
        {
            if (!type.IsGenericType)
            {
                return type.Name;
            }

            var typeName = type.Name.Split('`')[0];

            var genericArguments = type
                .GetGenericArguments()
                .Select(GetFriendlyTypeName)
                .Map(names => string.Join(", ", names));

            return $"{typeName}<{genericArguments}>";
        }
    }
}
