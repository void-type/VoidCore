using VoidCore.AspNet.Auth;
using Xunit;

namespace VoidCore.Test.AspNet.Auth;

public class AutoPolicyAuthorizeAttributeTests
{
    [Fact]
    public void Policy_is_set_based_on_attribute_name()
    {
        var att = new TesterOnly();
        Assert.Equal("Tester", att.Policy);
    }

    [Fact]
    public void Only_a_trailing_Only_is_removed_from_the_policy_name()
    {
        Assert.Equal("ReadOnlyUser", new ReadOnlyUser().Policy);
        Assert.Equal("ReadOnlyUsers", new ReadOnlyUsersOnly().Policy);
    }

    [Fact]
    public void Policy_name_is_case_insensitive_about_the_suffix()
    {
        Assert.Equal("Auditor", new Auditoronly().Policy);
    }

    [Fact]
    public void Attribute_without_a_name_before_the_suffix_is_rejected_instead_of_using_the_default_policy()
    {
        Assert.Throws<InvalidOperationException>(() => new Only());
    }

    private class TesterOnly : AutoPolicyAuthorizeAttribute { }
    private class ReadOnlyUser : AutoPolicyAuthorizeAttribute { }
    private class ReadOnlyUsersOnly : AutoPolicyAuthorizeAttribute { }
    private class Auditoronly : AutoPolicyAuthorizeAttribute { }
    private class Only : AutoPolicyAuthorizeAttribute { }
}
