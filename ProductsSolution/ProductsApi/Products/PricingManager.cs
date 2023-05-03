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

                Retail = PricingCalculations.CalculateRetailPrice(product, info),

                Wholesale = new ProductPricingWholeInformation
                {
                    Wholesale = PricingCalculations.CalculateWholesalePrice(product, info),
                    MinimumPurchaseRequired = PricingCalculations.CalculateMinimumPurchaseQty(product)
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

public static class PricingCalculations
{
    public static int CalculateMinimumPurchaseQty(CreateProductRequest product)
    {
        return product.Cost < 10 ? 10 : 5;
    }

    public static decimal CalculateWholesalePrice(CreateProductRequest product, SupplierPricingInformationResponse info)
    {
        return info.AllowWholesale ? product.Cost * 1.78M : product.Cost * .20M;
    }

    public static decimal CalculateRetailPrice(CreateProductRequest product, SupplierPricingInformationResponse info)
    {
        return info.RequiredMsrp.HasValue ? info.RequiredMsrp.Value : product.Cost * .20M;
    }
}