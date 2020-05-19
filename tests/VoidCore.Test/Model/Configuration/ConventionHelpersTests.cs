using System.Collections.Generic;
using VoidCore.Model.Configuration;
using Xunit;

namespace VoidCore.Test.Model.Configuration
{
    public class ConventionHelpersTests
    {
        [Fact]
        public void Strip_type_name_ending()
        {
            var typeName = typeof(MyBaseSettings).GetTypeNameWithoutEnding("Settings");
            Assert.Equal("MyBase", typeName);
        }

        [Fact]
        public void Strip_type_name_ending_is_case_insensitive()
        {
            var typeName2 = typeof(MyBaseSettings).GetTypeNameWithoutEnding("settings");
            Assert.Equal("MyBase", typeName2);

            var typeName1 = typeof(Lowercasesettings).GetTypeNameWithoutEnding("Settings");
            Assert.Equal("Lowercase", typeName1);
        }

        [Fact]
        public void No_match_in_type_name_returns_whole_name()
        {
            var typeName = typeof(Other).GetTypeNameWithoutEnding("Settings");
            Assert.Equal("Other", typeName);
        }

        [Fact]
        public void Null_search_in_type_name_returns_whole_name()
        {
            var sectionName = typeof(MyBaseSettings).GetTypeNameWithoutEnding(null);
            Assert.Equal("MyBaseSettings", sectionName);
        }

        [Fact]
        public void Friendly_type_name_returns_correct_name()
        {
            var typeName = typeof(string).GetFriendlyTypeName();
            Assert.Equal("String", typeName);
        }

        [Fact]
        public void Friendly_type_name_with_one_generic_parameter_returns_correct_name()
        {
            var typeName = typeof(List<string>).GetFriendlyTypeName();
            Assert.Equal("List<String>", typeName);
        }

        [Fact]
        public void Friendly_type_name_with_two_generic_parameters_returns_correct_name()
        {
            var typeName = typeof(Dictionary<string, MyBaseSettings>).GetFriendlyTypeName();
            Assert.Equal("Dictionary<String, MyBaseSettings>", typeName);
        }

        [Fact]
        public void Friendly_type_name_with_nested_generic_parameters_returns_correct_name()
        {
            var typeName = typeof(Dictionary<string, Dictionary<string, Dictionary<string, MyBaseSettings>>>).GetFriendlyTypeName();
            Assert.Equal("Dictionary<String, Dictionary<String, Dictionary<String, MyBaseSettings>>>", typeName);
        }
    }

    internal class MyBaseSettings { }

    internal class Lowercasesettings { }

    internal class Other { }
}
