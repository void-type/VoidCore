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

    private class TesterOnly : AutoPolicyAuthorizeAttribute { }
}
