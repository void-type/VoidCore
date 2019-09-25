using System;
using System.Collections.Generic;
using VoidCore.Domain.Guards;
using Xunit;

namespace VoidCore.Test.Domain
{
    public class GuardTests
    {
        [Fact]
        public void EnsureNotNull_throws_ArgumentNullException_when_value_is_null()
        {
            string myString = null;
            var ex = Assert.Throws<ArgumentNullException>(nameof(myString), () => myString.EnsureNotNull(nameof(myString)));
            Assert.Contains("Argument cannot be null.", ex.Message);
        }

        [Fact]
        public void EnsureNotNull_doesnt_throw_exception_when_value_not_null()
        {
            var myString = string.Empty;
            myString.EnsureNotNull(nameof(myString));
        }

        [Fact]
        public void EnsureNotNullOrEmpty_throws_ArgumentNullException_when_value_is_null()
        {
            string myString = null;
            var ex1 = Assert.Throws<ArgumentNullException>(nameof(myString), () => myString.EnsureNotNullOrEmpty(nameof(myString)));
            Assert.Contains("Argument cannot be null.", ex1.Message);

            List<string> myList = null;
            var ex2 = Assert.Throws<ArgumentNullException>(nameof(myList), () => myList.EnsureNotNullOrEmpty(nameof(myList)));
            Assert.Contains("Argument cannot be null.", ex2.Message);
        }

        [Fact]
        public void EnsureNotNullOrEmpty_throws_ArgumentException_when_value_is_empty()
        {
            var myString = string.Empty;
            var ex1 = Assert.Throws<ArgumentException>(nameof(myString), () => myString.EnsureNotNullOrEmpty(nameof(myString)));
            Assert.Contains("Argument cannot be empty.", ex1.Message);

            var myList = new List<string>();
            var ex2 = Assert.Throws<ArgumentException>(nameof(myList), () => myList.EnsureNotNullOrEmpty(nameof(myList)));
            Assert.Contains("Argument cannot be empty.", ex2.Message);

        }

        [Fact]
        public void EnsureNotNullOrEmpty_doesnt_throw_exception_when_value_not_null_or_empty()
        {
            var myString = "Something";
            myString.EnsureNotNullOrEmpty(nameof(myString));

            var myList = new List<string> { "Something" };
            myList.EnsureNotNullOrEmpty(nameof(myList));
        }

        [Fact]
        public void Ensure_throws_ArgumentException_when_condition_is_false()
        {
            const int myInt = 2;
            var ex = Assert.Throws<ArgumentException>(nameof(myInt), () => myInt.Ensure(i => i > 4, nameof(myInt)));
            Assert.Contains("Argument is invalid.", ex.Message);
        }

        [Fact]
        public void Ensure_doesnt_throw_exception_when_condition_is_true()
        {
            const int myInt = 2;
            myInt.Ensure(i => i < 4, nameof(myInt), i => "Int must be less than 4.");
        }

        [Fact]
        public void Ensure_with_message_builder_replaces_exception_message()
        {
            const int myInt = 2;
            var ex = Assert.Throws<ArgumentException>(nameof(myInt), () => myInt.Ensure(i => i > 4, nameof(myInt), i => $"Int must be greater than 4. Got {i}."));
            Assert.Contains("Int must be greater than 4. Got 2.", ex.Message);
        }
    }
}
