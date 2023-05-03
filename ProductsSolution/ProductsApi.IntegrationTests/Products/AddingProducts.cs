using Alba;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductsApi.Adapters;
using ProductsApi.IntegrationTests.Products.Fixtures;
using ProductsApi.Products;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace ProductsApi.IntegrationTests.Products;

// Note: There is also a "ICollectionFixture" 
public class AddingProducts : IClassFixture<ProductsDatabaseFixture>
{
    private readonly IAlbaHost _host;
    private readonly WireMockServer _mockServer;
    public AddingProducts(ProductsDatabaseFixture fixture, WireMockServer mockServer)
    {
        _host = fixture.AlbaHost;
        _mockServer = mockServer;
    }

    [Fact]
    public async Task CreatingAProduct()
    {
        var request = new CreateProductRequest
        {
            Name = "Super Deluxe Dandruff Shampoo",
            Cost = 18,
            Supplier = new SupplierInformation
            {
                Id = "bobs-shop",
                SKU = "19891"
            }
        };

        var expectedResponse = new CreateProductResponse
        {
            Slug = "super-deluxe-dandruff-shampoo",
            Pricing = new ProductPricingInformation
            {
                Retail = 42.23M,
                Wholesale = new ProductPricingWholeInformation
                {
                    Wholesale = 32.04M,
                    MinimumPurchaseRequired = 5
                }
            }

        };

        _mockServer.Given(Request.Create().WithPath($"/suppliers/{request.Supplier.Id}/products/{request.Supplier.SKU}"))
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBodyAsJson(new SupplierPricingInformationResponse
                {
                    AllowWholesale = true,
                    RequiredMsrp = 42.23M
                }
            ));


        var response = await _host.Scenario(api =>
        {
            api.Post.Json(request).ToUrl("/products");
            api.StatusCodeShouldBe(201);
            api.Header("location")
            .SingleValueShouldMatch(new System.Text.RegularExpressions.Regex("http://localhost/products/" + expectedResponse.Slug));
        });

        var actualResponse = response.ReadAsJson<CreateProductResponse>();
        Assert.NotNull(actualResponse);

        Assert.Equal(expectedResponse, actualResponse);

        // "Shallow Testing"
        //mockedDocumentSession.Verify(s => s.Insert(It.IsAny<CreateProductResponse>()), Times.Once);
        //mockedDocumentSession.Verify(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(1));

        var savedResonse = await _host.Scenario(api =>
        {
            api.Get.Url("/products/" + actualResponse.Slug);
            api.StatusCodeShouldBeOk(); // 200
        });
        var lookupResponseProduct = savedResonse.ReadAsJson<CreateProductResponse>();
        Assert.Equal(expectedResponse, lookupResponseProduct);
    }
}

public class AddingProductsFixture : ProductsDatabaseFixture
{
    protected override void ConfigureAdditionalServices(IServiceCollection services)
    {
        //            Id = "bobs-shop",
        // SKU = "19891"
        var stubbedResponse = new SupplierPricingInformationResponse
        {
            AllowWholesale = false,
            RequiredMsrp = 42.23M

        };
        var stubbedProductsApiAdapter = new Mock<PricingApiAdapter>(null);
        stubbedProductsApiAdapter.Setup(a => a.GetThePricingInformationAsync("bobs-shop", "19891"))
            .ReturnsAsync(stubbedResponse);

        services.AddScoped<PricingApiAdapter>(sp => stubbedProductsApiAdapter.Object);

    }
}