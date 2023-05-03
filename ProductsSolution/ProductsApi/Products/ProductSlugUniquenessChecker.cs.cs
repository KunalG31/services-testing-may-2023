using Marten;
using Microsoft.AspNetCore.Http;

namespace ProductsApi.Products;

public class ProductSlugUniquenessChecker : ICheckForUniqueValues
{
    private readonly IDocumentSession _session;

    public ProductSlugUniquenessChecker(IDocumentSession session)
    {
        _session = session;
    }

    public async Task<bool> IsUniqueAsync(string attempt)
    {
        var isFound = await _session.Query<CreateProductResponse>()
            .Where(p => p.Slug == attempt).AnyAsync();

        return !isFound;
    }
}
