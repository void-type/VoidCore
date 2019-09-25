using VoidCore.AspNet.Routing;
using Xunit;

namespace VoidCore.Test.AspNet.Routing
{
    public class ApiRouteAttributeTests
    {
        [Fact]
        public void Base_api_path_is_correct()
        {
            Assert.Equal("/api", ApiRouteAttribute.BasePath);
        }

        [Fact]
        public void Base_api_path_template_is_correct()
        {
            var route = new ApiRouteAttribute("controller");

            Assert.Equal("/api/controller", route.Template);
        }
    }
}
