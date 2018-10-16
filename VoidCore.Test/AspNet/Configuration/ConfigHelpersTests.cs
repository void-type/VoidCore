using VoidCore.AspNet.Configuration;
using Xunit;

namespace VoidCore.Test.AspNet.Configuration
{
    public class ConfigHelpersTests
    {
        [Fact]
        public void SectionNameByConventionWithSettingsSuffix()
        {
            var sectionName = ConfigHelpers.SectionNameFromSettingsClass<MyBaseSettings>();
            Assert.Equal("MyBase", sectionName);
        }

        [Fact]
        public void SectionNameByConventionWithSettingsSuffixIsCaseInsensitive()
        {
            var sectionName = ConfigHelpers.SectionNameFromSettingsClass<Lowercasesettings>();
            Assert.Equal("Lowercase", sectionName);
        }

        [Fact]
        public void SectionNameByConventionWithoutSettingsSuffix()
        {
            var sectionName = ConfigHelpers.SectionNameFromSettingsClass<Other>();
            Assert.Equal("Other", sectionName);
        }
    }

    class MyBaseSettings { }

    class Lowercasesettings { }

    class Other { }
}
