using VoidCore.AspNet.Auth;
using Xunit;

namespace VoidCore.Test.AspNet.Attributes
{
    public class AutoPolicyAuthorizeAttributeTests
    {
        [Fact]
        public void PolicyIsSetBasedOnAttributeName()
        {
            var att = new TesterOnly();
            Assert.Equal("Tester", att.Policy);
        }

        internal class TesterOnly : AutoPolicyAuthorizeAttribute { }
    }
}