using FluentAssertions;
using ProductsApi.Products;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ProductsApi.UnitTests.Models;

public class ProductsValidation
{
    [Fact]
    public void HasValidationAttributesOnNameProperty()
    {
        var model = new CreateProductRequest();

        var propName = nameof(CreateProductRequest.Name);

        var nameProp = typeof(CreateProductRequest).GetTypeInfo().GetProperty(propName);

        nameProp.Should().BeDecoratedWith<RequiredAttribute>();
        nameProp.Should().BeDecoratedWith<MinLengthAttribute>(x => x.Length == 3);
        nameProp.Should().BeDecoratedWith<MaxLengthAttribute>(x => x.Length == 100);
    }

    [Fact]
    public void TheProductsControllerIsAnApiController()
    {
        // Check to see if that class has the attribue [ApiController]
    }
}