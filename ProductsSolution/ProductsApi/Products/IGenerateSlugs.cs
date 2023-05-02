namespace ProductsApi.Products;

public interface IGenerateSlugs
{
    Task<string> GenerateSlugForAsync(string name);
}
