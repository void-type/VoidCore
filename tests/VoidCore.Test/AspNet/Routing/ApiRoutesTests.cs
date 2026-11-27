using Microsoft.AspNetCore.Http;
using VoidCore.AspNet.Routing;
using Xunit;

namespace VoidCore.Test.AspNet.Routing;

public class ApiRoutesTests
{
    [Fact]
    public void Base_api_path_is_correct()
    {
        Assert.Equal("/api", ApiRoutes.BasePath);
    }

    [Theory]
    [InlineData("/api", true)]
    [InlineData("/api/recipes", true)]
    [InlineData("/API/recipes", true)]
    [InlineData("/apiary", false)]
    [InlineData("/recipes/api", false)]
    [InlineData("/", false)]
    public void IsApiRequest_matches_the_base_path_as_a_segment(string path, bool expected)
    {
        var context = new DefaultHttpContext();
        context.Request.Path = path;

        Assert.Equal(expected, ApiRoutes.IsApiRequest(context));
    }
}
