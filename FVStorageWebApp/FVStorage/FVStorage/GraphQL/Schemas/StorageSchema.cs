using FVStorage.GraphQL.Mutations;
using FVStorage.GraphQL.Queries;
using GraphQL.Types;

namespace FVStorage.GraphQL.Schemas;

public class StorageSchema: Schema
{
    public StorageSchema(IFVStorageStorage db)
    {
        Query = new SupplyQuery(db);
        Mutation = new SupplyMutation(db);
    }
}