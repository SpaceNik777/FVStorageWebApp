using FVStorage.Entities;
using FVStorage.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;

namespace FVStorage.GraphQL.Mutations;

public class SupplyMutation : ObjectGraphType
{
    private readonly IFVStorageStorage _db;

    public SupplyMutation(IFVStorageStorage db)
    {
        this._db = db;

        Field<SupplyGraphType>(
            "createSupply",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "productCode" },
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "amount" },
                new QueryArgument<NonNullGraphType<DateGraphType>> { Name = "date" }
            ),
            resolve: context =>
            {
                var id = context.GetArgument<string>("id");
                var productCode = context.GetArgument<string>("productCode");
                var amount = context.GetArgument<int>("amount");
                var date = context.GetArgument<DateTime>("date");

                var productModel = db.FindProduct(productCode);
                var supply = new Supply
                {
                    Id = id,
                    ProductCode = productCode,
                    Amount = amount,
                    Date = date,
                    SupplyProduct = productModel
                };
                _db.CreateSupply(supply);
                return supply;
            }
        );
    }
}