namespace ProductsApi.Products;

public interface IManageTheProductCatalog
{
    Task<CreateProductResponse> AddProductAsync(CreateProductRequest request);
    Task<CreateProductResponse?> GetProductAsync(string slug);
}
