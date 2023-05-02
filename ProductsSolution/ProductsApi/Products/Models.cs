using System.ComponentModel.DataAnnotations;

namespace ProductsApi.Products;

/*
 * Name: the name of the product, as it should be displayed in our catalog.
Cost: the current cost of the product from the supplier
Supplier:
    - The ID of the supplier
    - the SKU of the product we are listing for the supplier */


public record CreateProductRequest
{
    [Required, MinLength(3), MaxLength(100)]
    public string Name { get; init; } = string.Empty;
    public decimal Cost { get; init; }
    public SupplierInformation Supplier { get; init; } = new();

}

public record SupplierInformation
{
    public string Id { get; init; } = string.Empty;
    public string SKU { get; init; } = string.Empty;
}


/*Name: the name of the product, as it should be displayed in our catalog.
Slug: The "sluggified name of the product"
Pricing Information:
    - The current pricing for this product.
        - Wholesale (if allowed by manufacturer):
            - wholesale price
            - minimum purchase required for this price
        - Retail price
*/

public record CreateProductResponse
{
    public string Slug { get; init; } = string.Empty;
    public ProductPricingInformation Pricing { get; init; } = new();
}

public record ProductPricingInformation
{
    public decimal Retail { get; set; }
    public ProductPricingWholeInformation Wholesale { get; init; } = new();
}

public record ProductPricingWholeInformation
{
    public decimal Wholesale { get; set; }
    public int MinimumPurchaseRequired { get; set; }
}