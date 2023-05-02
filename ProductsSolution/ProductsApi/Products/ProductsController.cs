using Microsoft.AspNetCore.Mvc;

namespace ProductsApi.Products;

[ApiController]
public class ProductsController : ControllerBase
{

    private readonly IManageTheProductCatalog _productCatalog;

    public ProductsController(IManageTheProductCatalog productCatalog)
    {
        _productCatalog = productCatalog;
    }

    [HttpPost("/products")]
    public async Task<ActionResult<CreateProductResponse>> AddProduct([FromBody] CreateProductRequest request)
    {

        // Write the Code I wish I had
        CreateProductResponse response = await _productCatalog.AddProductAsync(request);
        return StatusCode(201, response);
    }
}