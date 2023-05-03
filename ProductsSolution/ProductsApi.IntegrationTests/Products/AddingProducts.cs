using Alba;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductsApi.IntegrationTests.Products.Fixtures;
using ProductsApi.Products;

namespace ProductsApi.IntegrationTests.Products;

// Note: There is also a "ICollectionFixture" 
public class AddingProducts : IClassFixture<ProductsDatabaseFixture>
{
    private readonly IAlbaHost _host;
    public AddingProducts(ProductsDatabaseFixture fixture)
    {
        _host = fixture.AlbaHost;
    }

    [Fact]
    public async Task CreatingAProduct()
    {

        var request = new CreateProductRequest
        {
            Name = "Super Deluxe Dandruff Shampoo",
            Cost = 120.88M,
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
                    Wholesale = 40.23M,
                    MinimumPurchaseRequired = 10
                }
            }

        };
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