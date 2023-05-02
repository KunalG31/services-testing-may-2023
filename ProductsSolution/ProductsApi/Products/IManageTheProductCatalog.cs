namespace ProductsApi.Products;

public interface IManageTheProductCatalog
{
    Task<CreateProductResponse> AddProductAsync(CreateProductRequest request);
}
