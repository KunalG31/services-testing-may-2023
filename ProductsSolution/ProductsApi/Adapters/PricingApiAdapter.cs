using Microsoft.CodeAnalysis.Operations;

namespace ProductsApi.Adapters;

public class PricingApiAdapter
{
    private readonly HttpClient _httpClient;

    public PricingApiAdapter(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<SupplierPricingInformationResponse?> GetThePricingInformationAsync(string supplierId, string sku)
    {
        var response = await _httpClient.GetAsync($"/suppliers/{supplierId}/products/{sku}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<SupplierPricingInformationResponse>();

        return content;
    }
}

public record SupplierPricingInformationResponse
{
    public bool AllowWholesale { get; set; }
    public decimal? RequiredMsrp { get; set; }
}