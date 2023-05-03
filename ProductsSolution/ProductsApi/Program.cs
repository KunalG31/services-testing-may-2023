using Marten;
using ProductsApi.Adapters;
using ProductsApi.Demo;
using ProductsApi.Products;

// CreateBuilder adds the "standard" good defaults for EVERYTHING

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 198 services
builder.Services.AddSingleton<ISystemClock, SystemClock>(); // + 1
builder.Services.AddScoped<IManageTheProductCatalog, ProductManager>();
builder.Services.AddScoped<IManagePricing, PricingManager>();

var pricingApiUri = builder.Configuration.GetValue<string>("PricingApiUrl") ?? throw new ArgumentNullException("No Pricing API Url Configured");
builder.Services.AddHttpClient<PricingApiAdapter>(client =>
{
    client.BaseAddress = new Uri(pricingApiUri);
});

builder.Services.AddScoped<IGenerateSlugs, SlugGenerator>();
builder.Services.AddScoped<ICheckForUniqueValues, ProductSlugUniquenessChecker>();

var productsConnectionString = builder.Configuration.GetConnectionString("products") ?? throw new ArgumentNullException("Need a connection string for the products data base");

builder.Services.AddMarten(options =>
{
    options.Connection(productsConnectionString);
    if (builder.Environment.IsDevelopment())
    {
        options.AutoCreateSchemaObjects = Weasel.Core.AutoCreate.All;
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/demo", (ISystemClock clock) =>
{
    var currentTime = clock.GetCurrent();
    var response = new DemoResponse
    {
        Message = "Hello from the Api!",
        CreatedAt = currentTime,
        GettingCloseToQuittingTime = currentTime.Hour >= 16
    };
    return Results.Ok(response);
});

app.UseAuthorization();

app.MapControllers();

app.Run();