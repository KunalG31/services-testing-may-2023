namespace ProductsApi.Products;

public interface ICheckForUniqueValues
{
    Task<bool> IsUniqueAsync(string attempt);
}