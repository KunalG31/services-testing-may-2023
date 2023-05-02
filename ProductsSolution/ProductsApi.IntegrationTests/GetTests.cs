using Alba;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductsApi.Adapters;
using ProductsApi.Demo;

namespace ProductsApi.IntegrationTests.Demo;

public class GetTests
{
    [Fact]
    public async Task ReturnsCorrectDataAfterCutoffForGettingCloseToQuittingTime()
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
                var fakeClock = new Mock<ISystemClock>();
                fakeClock.Setup(x => x.GetCurrent()).Returns(expectedResponse.CreatedAt);
                sp.AddSingleton<ISystemClock>(p => fakeClock.Object);
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

    [Fact]
    public async Task ReturnsCorrectDataBeforeCutoffForGettingCloseToQuittingTime()
    {
        var expectedResponse = new DemoResponse
        {
            Message = "Hello from the Api!",
            CreatedAt = new DateTimeOffset(new DateTime(1969, 4, 20, 13, 59, 00), TimeSpan.FromHours(-4)),
            GettingCloseToQuittingTime = false
        };

        await using var host = await AlbaHost.For<Program>(options =>
        {
            options.ConfigureServices((context, sp) =>
            {
                var fakeClock = new Mock<ISystemClock>();
                fakeClock.Setup(x => x.GetCurrent()).Returns(expectedResponse.CreatedAt);
                sp.AddSingleton<ISystemClock>(p => fakeClock.Object);
            });
        });
    }
}

public class FakeTestingClockAfterCutoff : ISystemClock
{
    public DateTimeOffset GetCurrent()
    {
        return new DateTimeOffset(new DateTime(1969, 4, 20, 23, 59, 00), TimeSpan.FromHours(-4));
    }
}
public class FakeTestingClockBeforeCutoff : ISystemClock
{
    public DateTimeOffset GetCurrent()
    {
        return new DateTimeOffset(new DateTime(1969, 4, 20, 13, 59, 00), TimeSpan.FromHours(-4));
    }
}

