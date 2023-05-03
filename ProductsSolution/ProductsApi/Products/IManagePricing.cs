namespace ProductsApi.Products;

public interface IManagePricing
{
    Task<ProductPricingInformation> GetPricingInformationForAsync(CreateProductRequest supplier);
}
