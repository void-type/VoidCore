using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Queries;
using Xunit;

namespace VoidCore.Test.Model.Queries
{
    public class SearchCriteriaTests
    {
        [Fact]
        public void SearchObjectPropertiesForManyTermsArray()
        {
            var objectCollection = new List<TestObject>
            {
                new TestObject(null, "aaaaaaaaaaa"),
                new TestObject("aaaaaaaaaaa", ""),
                new TestObject("other", "aaaaaaaaaaa"),
                new TestObject("text and OTHER", "aaaaaaaaaaa"), // found
                new TestObject("aaaaaaaaaaa", "othertextother"), // found
                new TestObject("text", "aaaaaaaaaaa"),
                new TestObject("TEXT", "aaaaaaaaaaa"),
                new TestObject(" text \n", "aaaaaaaaaaa")
            }.AsQueryable();

            var terms = new [] { "text", "other" };

            var actual = objectCollection.Where(
                SearchCriteria.PropertiesContain<TestObject>(
                    new SearchTerms(terms),
                    obj => obj.StringProperty1,
                    obj => obj.StringProperty2
                )
            );

            Assert.Equal(2, actual.Count());
        }

        [Fact]
        public void SearchObjectPropertiesForManyTermsString()
        {
            var objectCollection = new List<TestObject>
            {
                new TestObject(null, "aaaaaaaaaaa"),
                new TestObject("aaaaaaaaaaa", ""),
                new TestObject("other", "aaaaaaaaaaa"),
                new TestObject("text and OTHER", "aaaaaaaaaaa"), // found
                new TestObject("aaaaaaaaaaa", "othertextother"), // found
                new TestObject("text", "aaaaaaaaaaa"),
                new TestObject("TEXT", "aaaaaaaaaaa"),
                new TestObject(" text \n", "aaaaaaaaaaa")
            }.AsQueryable();

            var terms = "text other  ";

            var actual = objectCollection.Where(
                SearchCriteria.PropertiesContain<TestObject>(
                    new SearchTerms(terms),
                    obj => obj.StringProperty1,
                    obj => obj.StringProperty2
                )
            );

            Assert.Equal(2, actual.Count());
        }

        [Fact]
        public void SearchObjectPropertiesForNoTermsReturnsAll()
        {
            var objectCollection = new List<TestObject>
            {
                new TestObject(null, "aaaaaaaaaaa"), // found
                new TestObject("aaaaaaaaaaa", ""), // found
                new TestObject("other", "aaaaaaaaaaa"), // found
                new TestObject("text and OTHER", "aaaaaaaaaaa"), // found
                new TestObject("aaaaaaaaaaa", "othertextother"), // found
                new TestObject("text", "aaaaaaaaaaa"), // found
                new TestObject("TEXT", "aaaaaaaaaaa"), // found
                new TestObject(" text \n", "aaaaaaaaaaa") // found
            }.AsQueryable();

            var terms = new string[0];

            var actual = objectCollection.Where(
                SearchCriteria.PropertiesContain<TestObject>(
                    new SearchTerms(terms),
                    obj => obj.StringProperty1,
                    obj => obj.StringProperty2
                )
            );

            Assert.Equal(8, actual.Count());
        }

        [Fact]
        public void SearchObjectPropertiesForNullArrayTermsReturnsAll()
        {
            var objectCollection = new List<TestObject>
            {
                new TestObject(null, "aaaaaaaaaaa"), // found
                new TestObject("aaaaaaaaaaa", ""), // found
                new TestObject("other", "aaaaaaaaaaa"), // found
                new TestObject("text and OTHER", "aaaaaaaaaaa"), // found
                new TestObject("aaaaaaaaaaa", "othertextother"), // found
                new TestObject("text", "aaaaaaaaaaa"), // found
                new TestObject("TEXT", "aaaaaaaaaaa"), // found
                new TestObject(" text \n", "aaaaaaaaaaa") // found
            }.AsQueryable();

            var actual = objectCollection.Where(
                SearchCriteria.PropertiesContain<TestObject>(
                    new SearchTerms(null),
                    obj => obj.StringProperty1,
                    obj => obj.StringProperty2
                )
            );

            Assert.Equal(8, actual.Count());
        }

        [Fact]
        public void SearchObjectPropertiesForNullTermsReturnsAll()
        {
            var objectCollection = new List<TestObject>
            {
                new TestObject(null, "aaaaaaaaaaa"), // found
                new TestObject("aaaaaaaaaaa", ""), // found
                new TestObject("other", "aaaaaaaaaaa"), // found
                new TestObject("text and OTHER", "aaaaaaaaaaa"), // found
                new TestObject("aaaaaaaaaaa", "othertextother"), // found
                new TestObject("text", "aaaaaaaaaaa"), // found
                new TestObject("TEXT", "aaaaaaaaaaa"), // found
                new TestObject(" text \n", "aaaaaaaaaaa") // found
            }.AsQueryable();

            var actual = objectCollection.Where(
                SearchCriteria.PropertiesContain<TestObject>(
                    new SearchTerms((string) null),
                    obj => obj.StringProperty1,
                    obj => obj.StringProperty2
                )
            );

            Assert.Equal(8, actual.Count());
        }

        [Fact]
        public void SearchObjectPropertiesForSimpleArrayTermsReturnsAll()
        {
            var objectCollection = new List<TestObject>
            {
                new TestObject(null, "aaaaaaaaaaa"), // found
                new TestObject("aaaaaaaaaaa", ""), // found
                new TestObject("other", "aaaaaaaaaaa"), // found
                new TestObject("text and OTHER", "aaaaaaaaaaa"), // found
                new TestObject("aaaaaaaaaaa", "othertextother"), // found
                new TestObject("text", "aaaaaaaaaaa"), // found
                new TestObject("TEXT", "aaaaaaaaaaa"), // found
                new TestObject(" text \n", "aaaaaaaaaaa") // found
            }.AsQueryable();

            string[] terms = { "a" };

            var actual = objectCollection.Where(
                SearchCriteria.PropertiesContain<TestObject>(
                    new SearchTerms(terms),
                    obj => obj.StringProperty1,
                    obj => obj.StringProperty2
                )
            );

            Assert.Equal(8, actual.Count());
        }

        [Fact]
        public void SearchObjectPropertiesForSimpleStringTermsReturnsAll()
        {
            var objectCollection = new List<TestObject>
            {
                new TestObject(null, "aaaaaaaaaaa"), // found
                new TestObject("aaaaaaaaaaa", ""), // found
                new TestObject("other", "aaaaaaaaaaa"), // found
                new TestObject("text and OTHER", "aaaaaaaaaaa"), // found
                new TestObject("aaaaaaaaaaa", "othertextother"), // found
                new TestObject("text", "aaaaaaaaaaa"), // found
                new TestObject("TEXT", "aaaaaaaaaaa"), // found
                new TestObject(" text \n", "aaaaaaaaaaa") // found
            }.AsQueryable();

            string terms = "a";

            var actual = objectCollection.Where(
                SearchCriteria.PropertiesContain<TestObject>(
                    new SearchTerms(terms),
                    obj => obj.StringProperty1,
                    obj => obj.StringProperty2
                )
            );

            Assert.Equal(8, actual.Count());
        }

        [Fact]
        public void SearchObjectPropertiesInCollectionForOneTerm()
        {
            var objectCollection = new List<TestObject>
            {
                new TestObject(null, "aaaaaaaaaaa"),
                new TestObject("aaaaaaaaaaa", ""),
                new TestObject("other", "aaaaaaaaaaa"),
                new TestObject("text and OTHER", "aaaaaaaaaaa"), // found
                new TestObject("aaaaaaaaaaa", "othertextother"), // found
                new TestObject("text", "aaaaaaaaaaa"), // found
                new TestObject("TEXT", "aaaaaaaaaaa"), // found
                new TestObject(" text \n", "aaaaaaaaaaa") // found
            }.AsQueryable();

            var terms = "text";

            var actual = objectCollection.Where(
                SearchCriteria.PropertiesContain<TestObject>(
                    new SearchTerms(terms),
                    obj => obj.StringProperty1,
                    obj => obj.StringProperty2
                )
            );

            Assert.Equal(5, actual.Count());
        }

        private class TestObject
        {
            public TestObject(string propertyValue1, string propertyValue2)
            {
                StringProperty1 = propertyValue1;
                StringProperty2 = propertyValue2;
            }

            public string StringProperty1 { get; }

            public string StringProperty2 { get; }
        }
    }
}
