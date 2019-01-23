using VoidCore.AspNet.Authorization;
using Xunit;

namespace VoidCore.Test.AspNet.Authorization
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
