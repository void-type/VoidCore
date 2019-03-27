using System;
using System.Collections.Generic;
using VoidCore.Domain.Guards;
using Xunit;

namespace VoidCore.Test.Domain
{
    public class GuardTests
    {
        [Fact]
        public void EnsureNotNullThrowsWhenValueNull()
        {
            string myString1 = null;
            var ex1 = Assert.Throws<ArgumentNullException>("myString1", () => myString1.EnsureNotNull(nameof(myString1)));
            Assert.Contains("Argument cannot be null.", ex1.Message);

            string myString2 = string.Empty;
            myString2.EnsureNotNull(nameof(myString2));
        }

        [Fact]
        public void EnsureNotNullOrEmptyStringThrowsWhenNullOrEmpty()
        {
            string myString1 = null;
            var ex1 = Assert.Throws<ArgumentNullException>("myString1", () => myString1.EnsureNotNullOrEmpty(nameof(myString1)));
            Assert.Contains("Argument cannot be null.", ex1.Message);

            string myString2 = string.Empty;
            var ex2 = Assert.Throws<ArgumentException>("myString2", () => myString2.EnsureNotNullOrEmpty(nameof(myString2)));
            Assert.Contains("Argument cannot be empty.", ex2.Message);

            string myString3 = "Something";
            myString3.EnsureNotNullOrEmpty(nameof(myString3));
        }

        [Fact]
        public void EnsureNotNullOrEmptyEnumerableThrowsWhenNullOrEmpty()
        {
            List<string> myStrings1 = null;
            var ex1 = Assert.Throws<ArgumentNullException>("myStrings1", () => myStrings1.EnsureNotNullOrEmpty(nameof(myStrings1)));
            Assert.Contains("Argument cannot be null.", ex1.Message);

            var myStrings2 = new List<string>();
            var ex2 = Assert.Throws<ArgumentException>("myStrings2", () => myStrings2.EnsureNotNullOrEmpty(nameof(myStrings2)));
            Assert.Contains("Argument cannot be empty.", ex2.Message);

            var myStrings3 = new List<string> { "Something" };
            myStrings3.EnsureNotNullOrEmpty(nameof(myStrings3));
        }

        [Fact]
        public void EnsureThrowsWhenConditionFalse()
        {
            var myInt1 = 2;
            var ex1 = Assert.Throws<ArgumentException>("myInt1", () => myInt1.Ensure(i => i > 4, nameof(myInt1)));
            Assert.Contains("Argument is invalid.", ex1.Message);

            myInt1.Ensure(i => i < 4, nameof(myInt1), "Int must be less than 4.");
        }

        [Fact]
        public void EnsureWithMessageBuilderThrowsWhenConditionFalse()
        {
            var myInt1 = 2;
            var ex = Assert.Throws<ArgumentException>("myInt1", () => myInt1.Ensure(i => i > 4, nameof(myInt1), i => $"Int must be greater than 4. Got {i}."));
            Assert.Contains("Int must be greater than 4. Got 2.", ex.Message);
        }
    }
}
