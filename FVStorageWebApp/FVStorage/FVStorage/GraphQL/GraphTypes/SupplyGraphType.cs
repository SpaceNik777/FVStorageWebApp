using FVStorage.Entities;
using GraphQL.Types;

namespace FVStorage.GraphQL.GraphTypes;

public sealed class SupplyGraphType : ObjectGraphType<Supply>
{
    public SupplyGraphType()
    {
        Name = "supply";
        Field(c => c.SupplyProduct, nullable: false, type: typeof(ProductGraphType))
            .Description("The product model of this particular supply");
        Field(c => c.Id);
        Field(c => c.ProductCode).Description("The product code of this particular supply");
        Field(c => c.Amount);
        Field(c => c.Date);
    }
}