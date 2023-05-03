namespace ProductsApi.Products;

public interface IManagePricing
{
    Task<ProductPricingInformation> GetPricingInformationForAsync(SupplierInformation supplier);
}
