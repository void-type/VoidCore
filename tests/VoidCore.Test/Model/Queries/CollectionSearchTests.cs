using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Queries;
using Xunit;

namespace VoidCore.Test.Model.Queries
{
    public class CollectionSearchTests
    {
        [Fact]
        public void ParseNullSearchTerms()
        {
            var actual = CollectionSearch.ParseStringForTerms(null);
            var expected = new string[0];
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ParseSearchTerms()
        {
            var actual = CollectionSearch.ParseStringForTerms("   one \n two\r\nthree  \n ");
            var expected = new [] { "one", "two", "three" };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ParseSearchTermsAllWhiteSpace()
        {
            var actual = CollectionSearch.ParseStringForTerms("    \n \r\n  \n ");
            var expected = new string[0];
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SearchObjectPropertiesInCollectionForOneTerm()
        {
            var objs = new List<TestObject>
            {
                new TestObject(null),
                new TestObject(""),
                new TestObject("other"),
                new TestObject("text and OTHER"), // found
                new TestObject("othertextother"), //found
                new TestObject("text"), // found
                new TestObject("TEXT"), // found
                new TestObject(" text \n") // found
            }.AsQueryable();

            var terms = "text";

            var actual = objs.SearchStringProperties(
                terms,
                obj => obj.StringProperty
            );

            Assert.Equal(5, actual.Count());
        }

        [Fact]
        public void SearchObjectPropertiesForManyTerms()
        {
            var objs = new List<TestObject>
            {
                new TestObject(null),
                new TestObject(""),
                new TestObject("other"),
                new TestObject("text and OTHER"), // found
                new TestObject("othertextother"), // found
                new TestObject("text"),
                new TestObject("TEXT"),
                new TestObject(" text \n")
            }.AsQueryable();

            var terms = new [] { "text", "other" };

            var actual = objs.SearchStringProperties(
                terms,
                obj => obj.StringProperty
            );

            Assert.Equal(2, actual.Count());
        }

        [Fact]
        public void SearchObjectPropertiesForNoTermsReturnsAll()
        {
            var objs = new List<TestObject>
            {
                new TestObject(null),
                new TestObject(""),
                new TestObject("other"),
                new TestObject("text and OTHER"),
                new TestObject("othertextother"),
                new TestObject("text"),
                new TestObject("TEXT"),
                new TestObject(" text \n")
            }.AsQueryable();

            var terms = new string[0];

            var actual = objs.SearchStringProperties(
                terms,
                obj => obj.StringProperty
            );

            Assert.Equal(objs.Count(), actual.Count());
        }

        private class TestObject
        {
            public TestObject(string propertyValue)
            {
                StringProperty = propertyValue;
            }

            public string StringProperty { get; }
        }
    }
}
