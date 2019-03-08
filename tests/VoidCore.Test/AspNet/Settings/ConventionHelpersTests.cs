using VoidCore.AspNet.Settings;
using Xunit;

namespace VoidCore.Test.AspNet.Settings
{
    public class ConventionHelpersTests
    {
        [Fact]
        public void SectionNameByConventionWithSettingsSuffix()
        {
            var sectionName = ConventionHelpers.StripEndingFromType(typeof(MyBaseSettings), "settings");
            Assert.Equal("MyBase", sectionName);
        }

        [Fact]
        public void SectionNameByConventionWithSettingsSuffixIsCaseInsensitive()
        {
            var sectionName = ConventionHelpers.StripEndingFromType(typeof(Lowercasesettings), "settings");
            Assert.Equal("Lowercase", sectionName);
        }

        [Fact]
        public void SectionNameByConventionWithoutSettingsSuffix()
        {
            var sectionName = ConventionHelpers.StripEndingFromType(typeof(Other), "settings");
            Assert.Equal("Other", sectionName);
        }

        [Fact]
        public void WholeSectionNameWhenEndingIsNull()
        {
            var sectionName = ConventionHelpers.StripEndingFromType(typeof(MyBaseSettings), null);
            Assert.Equal("MyBaseSettings", sectionName);
        }
    }

    internal class MyBaseSettings { }

    internal class Lowercasesettings { }

    internal class Other { }
}
