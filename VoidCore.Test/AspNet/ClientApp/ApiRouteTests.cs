using VoidCore.AspNet.ClientApp;
using Xunit;

namespace VoidCore.Test.AspNet.ClientApp
{
    public class ApiRouteTests
    {
        [Fact]
        public void BaseRouteIsCorrect()
        {
            Assert.Equal("/api", ApiRoute.BasePath);
        }

        [Fact]
        public void RoutePathTemplateIsCorrect()
        {
            var route = new ApiRoute("controller");

            Assert.Equal("/api/controller", route.Template);
        }
    }
}
