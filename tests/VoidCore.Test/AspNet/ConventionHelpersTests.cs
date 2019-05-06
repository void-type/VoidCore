using System;
using System.Collections.Generic;
using VoidCore.AspNet;
using Xunit;

namespace VoidCore.Test.AspNet
{
    public class ConventionHelpersTests
    {
        [Fact]
        public void TypeNameWithoutEndingStripsEnding()
        {
            var typeName = typeof(MyBaseSettings).GetTypeNameWithoutEnding("Settings");
            Assert.Equal("MyBase", typeName);
        }

        [Fact]
        public void TypeNameWithoutEndingIsCaseInsensitive()
        {
            var typeName2 = typeof(MyBaseSettings).GetTypeNameWithoutEnding("settings");
            Assert.Equal("MyBase", typeName2);

            var typeName1 = typeof(Lowercasesettings).GetTypeNameWithoutEnding("Settings");
            Assert.Equal("Lowercase", typeName1);
        }

        [Fact]
        public void TypeNameWithoutEndingWithoutMatchReturnsWholeName()
        {
            var typeName = typeof(Other).GetTypeNameWithoutEnding("Settings");
            Assert.Equal("Other", typeName);
        }

        [Fact]
        public void TypeNameWithoutEndingWithNullEndingReturnsWholeName()
        {
            var sectionName = typeof(MyBaseSettings).GetTypeNameWithoutEnding(null);
            Assert.Equal("MyBaseSettings", sectionName);
        }

        [Fact]
        public void FriendlyTypeNameWithoutGenericParameters()
        {
            var typeName = ConventionHelpers.GetFriendlyTypeName(typeof(String));
            Assert.Equal("String", typeName);
        }

        [Fact]
        public void FriendlyTypeNameWithOneGenericParameter()
        {
            var typeName = ConventionHelpers.GetFriendlyTypeName(typeof(List<string>));
            Assert.Equal("List<String>", typeName);
        }

        [Fact]
        public void FriendlyTypeNameWithTwoGenericParameters()
        {
            var typeName = ConventionHelpers.GetFriendlyTypeName(typeof(Dictionary<string, MyBaseSettings>));
            Assert.Equal("Dictionary<String, MyBaseSettings>", typeName);
        }

        [Fact]
        public void FriendlyTypeNameWithNestedGenericParameters()
        {
            var typeName = ConventionHelpers.GetFriendlyTypeName(typeof(Dictionary<string, Dictionary<string, Dictionary<string, MyBaseSettings>>>));
            Assert.Equal("Dictionary<String, Dictionary<String, Dictionary<String, MyBaseSettings>>>", typeName);
        }
    }

    internal class MyBaseSettings { }

    internal class Lowercasesettings { }

    internal class Other { }
}
