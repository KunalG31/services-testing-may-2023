using Alba;
using Microsoft.Extensions.DependencyInjection;
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
            CreatedAt = new DateTimeOffset(new DateTime(1969, 4, 20, 23, 59, 00), TimeSpan.FromHours(-4)),
            GettingCloseToQuittingTime = true
        };

        await using var host = await AlbaHost.For<Program>(options =>
        {
            options.ConfigureServices((context, sp) =>
            {
                sp.AddSingleton<ISystemClock, FakeTestingClock>();
            });
        });

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

public class FakeTestingClock : ISystemClock
{
    public DateTimeOffset GetCurrent()
    {
        return new DateTimeOffset(new DateTime(1969, 4, 20, 23, 59, 00), TimeSpan.FromHours(-4));
    }
}