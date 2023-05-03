using ProductsApi.Adapters;

namespace ProductsApi.Products;

public class PricingManager : IManagePricing
{
    private readonly PricingApiAdapter _adapter;

    public PricingManager(PricingApiAdapter adapter)
    {
        _adapter = adapter;
    }

    public async Task<ProductPricingInformation> GetPricingInformationForAsync(CreateProductRequest product)
    {

        var info = await _adapter.GetThePricingInformationAsync(product.Supplier.Id, product.Supplier.SKU);

        if (info is not null)
        {
            var response = new ProductPricingInformation
            {

                Retail = info.RequiredMsrp.HasValue ? info.RequiredMsrp.Value : product.Cost * .20M,

                Wholesale = new ProductPricingWholeInformation
                {
                    Wholesale = info.AllowWholesale ? product.Cost * 1.78M : product.Cost * .20M,
                    MinimumPurchaseRequired = product.Cost < 10 ? 10 : 5
                }

            };

            return response;
        }
        else
        {
            throw new Exception("Blammo");
        }

    }
}