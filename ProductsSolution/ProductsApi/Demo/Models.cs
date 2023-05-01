namespace ProductsApi.Demo;

public record DemoResponse
{
    public string Message { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }

}