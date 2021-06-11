using AvailabilityNotify.Services;
using GraphQL;
using GraphQL.Types;
using Newtonsoft.Json;
using Vtex.Api.Context;

namespace AvailabilityNotify.GraphQL
{
    [GraphQLMetadata("Mutation")]
    public class Mutation : ObjectGraphType<object>
    {
        public Mutation(IIOServiceContext context, IVtexAPIService vtexApiService, IAvailabilityRepository availabilityRepository)
        {
            Name = "Mutation";

            Field<BooleanGraphType>(
                "availabilitySubscribe",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> {Name = "name"},
                    new QueryArgument<StringGraphType> {Name = "email"},
                    new QueryArgument<StringGraphType> {Name = "skuId"}
                ),
                resolve: context =>
                {
                    var name = context.GetArgument<string>("name");
                    var email = context.GetArgument<string>("email");
                    var skuId = context.GetArgument<string>("skuId");

                    return vtexApiService.AvailabilitySubscribe(email, skuId, name);
                });

            Field<BooleanGraphType>(
                "deleteRequest",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> {Name = "id"}
                ),
                resolve: context =>
                {
                    var id = context.GetArgument<string>("id");

                    return availabilityRepository.DeleteNotifyRequest(id);
                });
        }
    }
}