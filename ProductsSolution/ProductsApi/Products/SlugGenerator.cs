namespace ProductsApi.Products;

public class SlugGenerator : IGenerateSlugs
{
    public async Task<string> GenerateSlugForAsync(string name)
    {
        return "blah";
    }
}