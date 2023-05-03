using Alba;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Builders;
using Testcontainers.PostgreSql;
using Marten;

namespace ProductsApi.IntegrationTests.Products.Fixtures;

public class ProductsDatabaseFixture : IAsyncLifetime
{
    private readonly string PG_IMAGE = "jeffrygonzalez/sdt-may-products-empty:latest";

    public IAlbaHost AlbaHost = null!;
    private readonly PostgreSqlContainer _pgContainer;

    public ProductsDatabaseFixture()
    {
        _pgContainer = new PostgreSqlBuilder()
        .WithDatabase("products_dev")
        .WithUsername("user")
        .WithPassword("password")
        .WithPortBinding(5432, 5432)
        .WithImage(PG_IMAGE)
        .Build();
    }

    public async Task InitializeAsync()
    {
        await _pgContainer.StartAsync();
        AlbaHost = await Alba.AlbaHost.For<Program>(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddMarten(options =>
                {
                    options.Connection(_pgContainer.GetConnectionString());
                });
            });
        });
    }

    public async Task DisposeAsync()
    {
        await AlbaHost.DisposeAsync();
        await _pgContainer.DisposeAsync();
    }
}