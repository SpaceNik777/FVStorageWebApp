using FVStorage.Entities;
using FVStorage.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;

namespace FVStorage.GraphQL.Queries;

public class SupplyQuery : ObjectGraphType
{
    private readonly IFVStorageStorage _db;

    public SupplyQuery(IFVStorageStorage db)
    {
        this._db = db;

        Field<ListGraphType<SupplyGraphType>>("Supplies", "Query to retrieve all Supplies",
            resolve: GetAllSupplies);
        
        Field<SupplyGraphType>("Supply", "Query to retrieve a specific Suplly",
            new QueryArguments(MakeNonNullStringArgument("id", "The id of the Supply")),
            resolve: GetSupply);
        
        Field<SupplyGraphType>("SuppliesByDate", "Query to retrieve all Supllies matching the specified date",
            new QueryArguments(MakeNonNullStringArgument("date", "The date")),
            resolve: GetSuppliesByDate);

    }
    
    private QueryArgument MakeNonNullStringArgument(string name, string description) {
        return new QueryArgument<NonNullGraphType<StringGraphType>> {
            Name = name, Description = description
        };
    }

    private IEnumerable<Supply> GetAllSupplies(IResolveFieldContext<object> context) => _db.ListSupplies();

    private Supply GetSupply(IResolveFieldContext<object> context)
    {
        var id = context.GetArgument<String>("id");
        return _db.FindSupply(id);
    }

    private IEnumerable<Supply> GetSuppliesByDate(IResolveFieldContext<object> context)
    {
        var date = context.GetArgument<DateTime>("date");
        var supplies = _db.ListSupplies()
            .Where(s => s.Date.Equals(date));
        return supplies;
    }

}