using Alba;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Builders;
using Testcontainers.PostgreSql;
using Marten;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        //.WithPortBinding(5432, 5432) // Don't do this because you'll have multiple running.
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
                ConfigureAdditionalServices(services);
                services.AddMarten(options =>
                {
                    var connectionString = _pgContainer.GetConnectionString();
                    options.Connection(connectionString);
                });
            });
        });
    }

    protected virtual void ConfigureAdditionalServices(IServiceCollection services)
    {
        //
    }

    public async Task DisposeAsync()
    {
        await AlbaHost.DisposeAsync();
        await _pgContainer.DisposeAsync().AsTask();
    }

    
}