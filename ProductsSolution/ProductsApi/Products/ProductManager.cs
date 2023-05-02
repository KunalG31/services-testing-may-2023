namespace ProductsApi.Products;

public class ProductManager : IManageTheProductCatalog
{
    private readonly IGenerateSlugs _slugGenerator;

    public ProductManager(IGenerateSlugs slugGenerator)
    {
        _slugGenerator = slugGenerator;
    }

    public async Task<CreateProductResponse> AddProductAsync(CreateProductRequest request)
    {
        var response = new CreateProductResponse
        {
            Slug = await _slugGenerator.GenerateSlugForAsync(request.Name),
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
        return response;
    }
}