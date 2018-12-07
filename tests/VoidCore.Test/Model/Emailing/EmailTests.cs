using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Emailing;
using Xunit;

namespace VoidCore.Test.Model.Emailing
{
    public class EmailTests
    {
        [Fact]
        public void EmailCreation()
        {
            var email = new Email("RE: Testing", "Please test.", new List<string>() { "vt@example.com", "vt2@example.com" });

            Assert.Equal("RE: Testing", email.Subject);
            Assert.Equal("Please test.", email.Message);
            Assert.Equal(2, email.Recipients.Count());
            Assert.Contains("vt@example.com", email.Recipients);
            Assert.Contains("vt2@example.com", email.Recipients);
        }
    }
}
