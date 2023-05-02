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
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IGenerateSlugs, SlugGenerator>();
    builder.Services.AddScoped<ICheckForUniqueValues, ProductSlugUniquenessChecker>();
}
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