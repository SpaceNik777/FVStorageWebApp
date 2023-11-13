using FVStorage.Entities;
using GraphQL.Types;

namespace FVStorage.GraphQL.GraphTypes;

public sealed class ProductGraphType: ObjectGraphType<Product>
{
    public ProductGraphType()
    {
        Name = "product";
        Field(m => m.Name).Description("The name of this Product");
        Field(m => m.Weight).Description("The weight of one product unit");
        Field(m => m.Price).Description("The wholesale price for one product unit");
        Field(m => m.Supplier).Description("The supplier of this product");
    }
}