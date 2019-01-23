using VoidCore.AspNet.Routing;
using Xunit;

namespace VoidCore.Test.AspNet.Routing
{
    public class ApiRouteAttributeTests
    {
        [Fact]
        public void BaseRouteIsCorrect()
        {
            Assert.Equal("/api", ApiRouteAttribute.BasePath);
        }

        [Fact]
        public void RoutePathTemplateIsCorrect()
        {
            var route = new ApiRouteAttribute("controller");

            Assert.Equal("/api/controller", route.Template);
        }
    }
}
