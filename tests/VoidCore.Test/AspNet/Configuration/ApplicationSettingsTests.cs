using VoidCore.AspNet.Configuration;
using Xunit;

namespace VoidCore.Test.AspNet.Configuration
{
    public class ApplicationSettingsTests
    {
        [Fact]
        public void Application_settings_has_parameterless_constructor_for_aspnet_options()
        {
            var appSettings = new ApplicationSettings();

            Assert.True(appSettings is ApplicationSettings);
        }
    }
}
