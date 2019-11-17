using System;
using System.Linq;
using VoidCore.Model.Emailing;
using Xunit;

namespace VoidCore.Test.Model.Emailing
{
    public class EmailFactoryTests
    {
        [Fact]
        public void TextEmailFactory_makes_text_based_emails()
        {
            var emailFactory = new TextEmailFactory();

            var email = emailFactory.Create(email =>
            {
                email.SetSubject("my subject");

                email.AddLine("line 1");
                email.AddLine("line 2");
                email.AddLine("line 3");
                email.AddLine("line 4");

                email.AddRecipient("recipient1");
                email.AddRecipient("recipient2");
            });

            Assert.Equal("my subject", email.Subject);
            Assert.Equal("line 1\r\nline 2\r\nline 3\r\nline 4", email.Message);
            Assert.Equal(2, email.Recipients.ToList().Count);
            Assert.Contains("recipient1", email.Recipients);
            Assert.Contains("recipient2", email.Recipients);
        }

        [Fact]
        public void HtmlEmailFactory_makes_text_based_emails()
        {
            var emailFactory = new HtmlEmailFactory();

            var email = emailFactory.Create(email =>
            {
                email.SetSubject("my subject");

                email.AddLine("line 1");
                email.AddLine("line 2");
                email.AddLine("line 3");
                email.AddLine("line 4");

                email.AddRecipient("recipient1");
                email.AddRecipient("recipient2");
            });

            Assert.Equal("my subject", email.Subject);
            Assert.Equal("<html><body>line 1<br>line 2<br>line 3<br>line 4</body></html>", email.Message);
            Assert.Equal(2, email.Recipients.ToList().Count);
            Assert.Contains("recipient1", email.Recipients);
            Assert.Contains("recipient2", email.Recipients);
        }

        [Fact]
        public void HtmlEm34ailBuilder_makes_text_based_emails()
        {
            var emailFactory = new HtmlEmailFactory();

            var email = emailFactory.Create(email =>
            {
                email.SetSubject("my subject");

                email.AddLine("line 1");
                email.AddLine("line 2");
                email.AddLine("line 3");
                email.AddLine("line 4");

                email.AddRecipient("recipient1");
                email.AddRecipient("recipient2");
            });

            Assert.Equal("my subject", email.Subject);
            Assert.Equal("<html><body>line 1<br>line 2<br>line 3<br>line 4</body></html>", email.Message);
            Assert.Equal(2, email.Recipients.ToList().Count);
            Assert.Contains("recipient1", email.Recipients);
            Assert.Contains("recipient2", email.Recipients);
        }

        [Fact]
        public void EmailOptionsBuilder_returns_non_null_parameters_if_nothing_configured()
        {
            var emailFactory = new TextEmailFactory();

            var email = emailFactory.Create(email => { });

            Assert.NotNull(email.Subject);
            Assert.NotNull(email.Message);
            Assert.NotNull(email.Recipients);

            Assert.Empty(email.Subject);
            Assert.Empty(email.Message);
            Assert.Empty(email.Recipients);
        }

        [Fact]
        public void EmailOptionsBuilder_throws_argument_exception_if_subject_or_recipient_is_null_or_empty()
        {
            var emailFactory = new HtmlEmailFactory();

            Assert.Throws<ArgumentException>(() =>
               emailFactory.Create(email =>
               {
                   email.SetSubject("");
               }));

            Assert.Throws<ArgumentException>(() =>
               emailFactory.Create(email =>
               {
                   email.AddRecipient("");
               }));

            Assert.Throws<ArgumentNullException>(() =>
               emailFactory.Create(email =>
               {
                   email.SetSubject(null);
               }));

            Assert.Throws<ArgumentNullException>(() =>
               emailFactory.Create(email =>
               {
                   email.AddRecipient(null);
               }));
        }

        [Fact]
        public void EmailOptionsBuilder_accepts_null_or_empty_body_lines_as_extra_line_breaks()
        {
            var emailFactory = new HtmlEmailFactory();

            var email = emailFactory.Create(email =>
            {
                email.AddLine("");
                email.AddLine();
                email.AddLine(null);
                email.AddLine("line 4");
            });

            Assert.Equal("<html><body><br><br><br>line 4</body></html>", email.Message);
        }
    }
}
