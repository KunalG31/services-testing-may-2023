using ProductsApi.Adapters;
using ProductsApi.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsApi.UnitTests;

public class DoingPricingCalculations
{
    [Theory]
    [InlineData(10.00, 5)]
    [InlineData(10.01, 5)]
    [InlineData(9.99, 10)]
    public void CalculatingMinimimumPurchaseOnWholesale(decimal cost, int qty)
    {
        var request = new CreateProductRequest
        {
            Cost = cost
        };
        Assert.Equal(qty, PricingCalculations.CalculateMinimumPurchaseQty(request));
    }

    [Fact]
    public void Banna()
    {
        //public static decimal CalculateWholesalePrice(CreateProductRequest product, SupplierPricingInformationResponse info)
        //{
        //    return info.AllowWholesale ? product.Cost * 1.78M : product.Cost * .20M;
        //}

        var product = new CreateProductRequest
        {
            Cost = 10M
        };
        var info = new SupplierPricingInformationResponse
        {
            AllowWholesale = true
        };
        var answer = PricingCalculations.CalculateWholesalePrice(product, info);

        Assert.Equal(17.80M, answer);
    }
}