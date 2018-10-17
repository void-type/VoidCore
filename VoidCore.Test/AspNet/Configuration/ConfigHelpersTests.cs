using VoidCore.AspNet.Configuration;
using Xunit;

namespace VoidCore.Test.AspNet.Configuration
{
    public class ConfigHelpersTests
    {
        [Fact]
        public void SectionNameByConventionWithSettingsSuffix()
        {
            var sectionName = ConfigHelpers.StripEndingFromType(typeof(MyBaseSettings), "settings");
            Assert.Equal("MyBase", sectionName);
        }

        [Fact]
        public void SectionNameByConventionWithSettingsSuffixIsCaseInsensitive()
        {
            var sectionName = ConfigHelpers.StripEndingFromType(typeof(Lowercasesettings), "settings");
            Assert.Equal("Lowercase", sectionName);
        }

        [Fact]
        public void SectionNameByConventionWithoutSettingsSuffix()
        {
            var sectionName = ConfigHelpers.StripEndingFromType(typeof(Other), "settings");
            Assert.Equal("Other", sectionName);
        }

        [Fact]
        public void WholeSectionNameWhenEndingIsNull()
        {
            var sectionName = ConfigHelpers.StripEndingFromType(typeof(MyBaseSettings), null);
            Assert.Equal("MyBaseSettings", sectionName);
        }
    }

    class MyBaseSettings { }

    class Lowercasesettings { }

    class Other { }
}
