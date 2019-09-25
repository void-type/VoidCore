using System;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Emailing;
using Xunit;

namespace VoidCore.Test.Model.Emailing
{
    public class EmailTests
    {
        [Fact]
        public void Email_can_be_created()
        {
            var email = new Email("RE: Testing", "Please test.", new List<string> { "vt@example.com", "vt2@example.com" });

            Assert.Equal("RE: Testing", email.Subject);
            Assert.Equal("Please test.", email.Message);
            Assert.Equal(2, email.Recipients.Count());
            Assert.Contains("vt@example.com", email.Recipients);
            Assert.Contains("vt2@example.com", email.Recipients);
        }

        [Fact]
        public void Email_can_be_created_with_empty_recipients_list()
        {
            var email = new Email("RE: Testing", "Please test.", new List<string>());

            Assert.Equal("RE: Testing", email.Subject);
            Assert.Equal("Please test.", email.Message);
            Assert.Empty(email.Recipients);
        }

        [Fact]
        public void Email_creation_with_null_subject_throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Email(null, "Please test.", new List<string> { "vt@example.com", "vt2@example.com" }));
        }

        [Fact]
        public void Email_creation_with_null_message_throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Email("RE: Testing", null, new List<string> { "vt@example.com", "vt2@example.com" }));
        }

        [Fact]
        public void Email_creation_with_null_recipients_throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Email("RE: Testing", "Please test.", null));
        }
    }
}
