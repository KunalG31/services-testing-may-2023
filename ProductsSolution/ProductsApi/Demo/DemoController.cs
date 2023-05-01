using Microsoft.AspNetCore.Mvc;
namespace ProductsApi.Demo;

public class DemoController : ControllerBase
{

    [HttpGet("/demo")]
    public async Task<ActionResult> GetTheDemo()
    {
        var response = new DemoResponse
        {
            Message = "Hello from the Api!",
            CreatedAt = DateTimeOffset.Now
        };
        return Ok(response);
    }
}