using FVStorage.Entities;
using GraphQL.Types;

namespace FVStorage.GraphQL.GraphTypes;

public sealed class SupplierGraphType: ObjectGraphType<Supplier>
{
    public SupplierGraphType()
    {
        Name = "supplier";
        Field(c => c.Name).Description("The name of this supplier");
    }
}