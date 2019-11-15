using System;
using System.Linq;
using VoidCore.Model.Emailing;
using Xunit;

namespace VoidCore.Test.Model.Emailing
{
    public class EmailBuilderTests
    {
        [Fact]
        public void TextEmailBuilder_makes_text_based_emails()
        {
            var emailBuilder = new TextEmailBuilder();

            var email = emailBuilder.Build(options =>
            {
                options.SetSubject("my subject");

                options.AddLine("line 1");
                options.AddLine("line 2");
                options.AddLine("line 3");
                options.AddLine("line 4");

                options.AddRecipient("recipient1");
                options.AddRecipient("recipient2");
            });

            Assert.Equal("my subject", email.Subject);
            Assert.Equal("line 1\r\nline 2\r\nline 3\r\nline 4", email.Message);
            Assert.Equal(2, email.Recipients.ToList().Count);
            Assert.Contains("recipient1", email.Recipients);
            Assert.Contains("recipient2", email.Recipients);
        }

        [Fact]
        public void HtmlEmailBuilder_makes_text_based_emails()
        {
            var emailBuilder = new HtmlEmailBuilder();

            var email = emailBuilder.Build(options =>
            {
                options.SetSubject("my subject");

                options.AddLine("line 1");
                options.AddLine("line 2");
                options.AddLine("line 3");
                options.AddLine("line 4");

                options.AddRecipient("recipient1");
                options.AddRecipient("recipient2");
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
            var emailBuilder = new HtmlEmailBuilder();

            var email = emailBuilder.Build(options =>
            {
                options.SetSubject("my subject");

                options.AddLine("line 1");
                options.AddLine("line 2");
                options.AddLine("line 3");
                options.AddLine("line 4");

                options.AddRecipient("recipient1");
                options.AddRecipient("recipient2");
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
            var emailBuilder = new TextEmailBuilder();

            var email = emailBuilder.Build(options => { });

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
            var emailBuilder = new HtmlEmailBuilder();

            Assert.Throws<ArgumentException>(() =>
                emailBuilder.Build(options =>
                {
                    options.SetSubject("");
                }));

            Assert.Throws<ArgumentException>(() =>
                emailBuilder.Build(options =>
                {
                    options.AddRecipient("");
                }));

            Assert.Throws<ArgumentNullException>(() =>
                emailBuilder.Build(options =>
                {
                    options.SetSubject(null);
                }));

            Assert.Throws<ArgumentNullException>(() =>
                emailBuilder.Build(options =>
                {
                    options.AddRecipient(null);
                }));
        }

        [Fact]
        public void EmailOptionsBuilder_accepts_null_or_empty_body_lines_as_extra_line_breaks()
        {
            var emailBuilder = new HtmlEmailBuilder();

            var email = emailBuilder.Build(options =>
            {
                options.AddLine("");
                options.AddLine();
                options.AddLine(null);
                options.AddLine("line 4");
            });

            Assert.Equal("<html><body><br><br><br>line 4</body></html>", email.Message);
        }
    }
}
