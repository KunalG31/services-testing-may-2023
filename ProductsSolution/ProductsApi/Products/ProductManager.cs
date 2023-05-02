using Marten;

namespace ProductsApi.Products;

public class ProductManager : IManageTheProductCatalog
{
    private readonly IGenerateSlugs _slugGenerator;
    private readonly IDocumentSession _session;

    public ProductManager(IGenerateSlugs slugGenerator, IDocumentSession session)
    {
        _slugGenerator = slugGenerator;
        _session = session;
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
        _session.Insert(response);
        await _session.SaveChangesAsync();
        return response;
    }

    public async Task<CreateProductResponse?> GetProductAsync(string slug)
    {
        var response = await _session.Query<CreateProductResponse>().Where(p => p.Slug == slug).SingleOrDefaultAsync();
        return response;
    }
}