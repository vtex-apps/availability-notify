using GraphQL;
using GraphQL.Types;
using Newtonsoft.Json;
using Vtex.Api.Context;

namespace AvailabilityNotify.GraphQL
{
    [GraphQLMetadata("Query")]
    public class Query : ObjectGraphType<object>
    {
        public Query(IIOServiceContext context)
        {
            Name = "Query";

            
        }
    }
}