using Alba;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ProductsApi.Demo;

namespace ProductsApi.IntegrationTests.Demo;

public class GetTests
{
    [Fact]
    public async Task GivesA200StatusCode()
    {
        var expectedResponse = new DemoResponse
        {
            Message = "Hello from the Api!",
            CreatedAt = DateTimeOffset.Now,
        };

        await using var host = await AlbaHost.For<Program>();

        // "Scenarios"
        var response = await host.Scenario(api =>
        {
            api.Get.Url("/demo");
            api.StatusCodeShouldBeOk();
            api.Header("content-type").ShouldHaveValues("application/json; charset=utf-8");
        });

        var actualResponse = response.ReadAsJson<DemoResponse>();

        Assert.Equal(expectedResponse, actualResponse);
        //Assert.Equal(expectedResponse.Message, actualResponse.Message);
    }
}