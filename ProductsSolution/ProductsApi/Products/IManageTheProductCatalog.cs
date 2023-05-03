namespace ProductsApi.Products;

public interface IManageTheProductCatalog
{
    Task<CreateProductResponse> AddProductAsync(CreateProductRequest request);
    Task<IList<CreateProductResponse>> GetAllAsync();
    Task<CreateProductResponse?> GetProductAsync(string slug);
}
