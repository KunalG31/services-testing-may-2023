
using Alba;
using LamarCodeGeneration.Frames;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductsApi.Products;

namespace ProductsApi.IntegrationTests.Products;

public class AddingProducts
{
    [Fact]
    public async Task CreatingAProduct()
    {
        var mockedDocumentSession = new Mock<IDocumentSession>();
        await using var host = await AlbaHost.For<Program>(options =>
        {
            options.ConfigureServices((context, sp) =>
            {
                //sp.AddScoped<IDocumentSession>(sp =>
                //{
                //    return mockedDocumentSession.Object;
                //});
            });
        });

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
        var response = await host.Scenario(api =>
        {
            api.Post.Json(request).ToUrl("/products");
            api.StatusCodeShouldBe(201);
        });

        var actualResponse = response.ReadAsJson<CreateProductResponse>();
        Assert.NotNull(actualResponse);

        Assert.Equal(expectedResponse, actualResponse);

        //mockedDocumentSession.Verify(s => s.Insert(It.IsAny<CreateProductResponse>()), Times.Once);
        //mockedDocumentSession.Verify(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(1));
    }
}