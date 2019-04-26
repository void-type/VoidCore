using VoidCore.AspNet.Auth;
using Xunit;

namespace VoidCore.Test.AspNet.Auth
{
    public class AutoPolicyAuthorizeAttributeTests
    {
        [Fact]
        public void PolicyIsSetBasedOnAttributeName()
        {
            var att = new TesterOnly();
            Assert.Equal("Tester", att.Policy);
        }

        private class TesterOnly : AutoPolicyAuthorizeAttribute { }
    }
}
