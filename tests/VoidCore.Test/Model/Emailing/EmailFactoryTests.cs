using VoidCore.Model.Emailing;
using Xunit;

namespace VoidCore.Test.Model.Emailing;

public class EmailFactoryTests
{
    [Fact]
    public void TextEmailFactory_makes_text_based_emails()
    {
        var emailFactory = new TextEmailFactory();

        var email = emailFactory.Create(builder =>
        {
            builder.SetSubject("my subject");

            builder.AddLine("line 1");
            builder.AddLine("line 2");
            builder.AddLines("line 3", "line 4");

            builder.AddRecipients("recipient1", "recipient2");
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

        var email = emailFactory.Create(builder =>
        {
            builder.SetSubject("my subject");

            builder.AddLine("line 1");
            builder.AddLine("line 2");
            builder.AddLines(new List<string>() { "line 3", "line 4" });

            builder.AddRecipients(new List<string> { "recipient1", "recipient2" });
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

        var email = emailFactory.Create(builder =>
        {
            builder.SetSubject("my subject");

            builder.AddLine("line 1");
            builder.AddLine("line 2");
            builder.AddLines("line 3", "line 4");

            builder.AddRecipients("recipient1", "recipient2");
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

        var email = emailFactory.Create(builder => { });

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
               email.SetSubject(null!);
           }));

        Assert.Throws<ArgumentNullException>(() =>
           emailFactory.Create(email =>
           {
               email.AddRecipient(null!);
           }));
    }

    [Theory]
    [InlineData("subject\r\nBcc: attacker@example.com")]
    [InlineData("subject\ninjected")]
    public void Subject_with_line_breaks_is_rejected_to_prevent_header_injection(string subject)
    {
        var factory = new TextEmailFactory();

        Assert.Throws<ArgumentException>(() => factory.Create(builder => builder.SetSubject(subject)));
        Assert.Throws<ArgumentException>(() => new Email(subject, "message", "a@example.com"));
    }

    [Theory]
    [InlineData("a@example.com\r\nBcc: attacker@example.com")]
    [InlineData("a@example.com\ninjected")]
    [InlineData("   ")]
    public void Invalid_recipients_are_rejected(string recipient)
    {
        var factory = new TextEmailFactory();

        Assert.Throws<ArgumentException>(() => factory.Create(builder => builder.AddRecipient(recipient)));
        Assert.Throws<ArgumentException>(() => new Email("subject", "message", recipient));
    }

    [Fact]
    public void Email_copies_the_recipients()
    {
        var recipients = new List<string> { "a@example.com" };

        var email = new Email("subject", "message", recipients);
        recipients.Add("b@example.com");

        Assert.Equal(["a@example.com"], email.Recipients);
    }

    [Fact]
    public void EmailOptionsBuilder_accepts_null_or_empty_body_lines_as_extra_line_breaks()
    {
        var emailFactory = new HtmlEmailFactory();

        var email = emailFactory.Create(builder =>
        {
            builder.AddLine(string.Empty);
            builder.AddLine();
            builder.AddLine(null!);
            builder.AddLine("line 4");
        });

        Assert.Equal("<html><body><br><br><br>line 4</body></html>", email.Message);
    }
}
