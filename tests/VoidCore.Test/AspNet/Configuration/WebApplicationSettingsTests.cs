using System;
using VoidCore.AspNet.Configuration;
using Xunit;

namespace VoidCore.Test.AspNet.Configuration
{
    public class WebApplicationSettingsTests
    {
        [Fact]
        public void WebApplicationSettings_has_parameterless_constructor_for_aspnet_options()
        {
            var appSettings = new WebApplicationSettings()
            {
                Name = "Something",
                BaseUrl = "Something"
            };

            appSettings.Validate();

            Assert.True(appSettings is WebApplicationSettings);
        }

        [Fact]
        public void WebApplicationSettings_validator_throws_when_settings_invalid()
        {
            Assert.Throws<ArgumentException>(() => new WebApplicationSettings().Validate());
            Assert.Throws<ArgumentException>(() => new WebApplicationSettings() { Name = "Something" }.Validate());
        }
    }
}
