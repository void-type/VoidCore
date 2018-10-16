namespace VoidCore.AspNet.Configuration
{
    /// <summary>
    /// Helpers to assist in configuring the application.
    /// </summary>
    public static class ConfigHelpers
    {
        /// <summary>
        /// Returns the section name based on the name of the settings class.
        /// Ex: AuthorizationSettings => "Authorization"
        /// Ex: Authorization => "Authorization"
        /// </summary>
        /// <typeparam name="TSettings">The type of settings</typeparam>
        /// <returns></returns>
        public static string SectionNameFromSettingsClass<TSettings>()
        {
            var rawName = typeof(TSettings).Name;
            var nameEnd = rawName.ToLower().LastIndexOf("settings");
            if (nameEnd < 0)
            {
                nameEnd = rawName.Length;
            }
            return rawName.Substring(0, nameEnd);
        }
    }
}
