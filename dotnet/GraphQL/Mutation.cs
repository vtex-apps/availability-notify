using AvailabilityNotify.GraphQL.Types;
using AvailabilityNotify.Models;
using AvailabilityNotify.Services;
using GraphQL;
using GraphQL.Types;
using Vtex.Api.Context;
using System.Net;

namespace AvailabilityNotify.GraphQL
{
    [GraphQLMetadata("Mutation")]
    public class Mutation : ObjectGraphType<object>
    {
        public Mutation(IIOServiceContext contextService, IVtexAPIService vtexApiService, IAvailabilityRepository availabilityRepository)
        {
            Name = "Mutation";

            Field<BooleanGraphType>(
                "availabilitySubscribe",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> {Name = "name"},
                    new QueryArgument<StringGraphType> {Name = "email"},
                    new QueryArgument<StringGraphType> {Name = "skuId"},
                    new QueryArgument<StringGraphType> {Name = "locale"},
                    new QueryArgument<NonNullGraphType<SellerObjInputType>> {Name = "sellerObj"}
                ),
                resolve: context =>
                {
                    var name = context.GetArgument<string>("name");
                    var email = context.GetArgument<string>("email");
                    var skuId = context.GetArgument<string>("skuId");
                    var locale = context.GetArgument<string>("locale");
                    var sellerObj = context.GetArgument<SellerObj>("sellerObj");
                    contextService.Vtex.Logger.Debug("GraphQL", null, $"AvailabilitySubscribe Mutation called: '{name}' '{email}' '{skuId}' '{locale}'");

                    return vtexApiService.AvailabilitySubscribe(email, skuId, name, locale, sellerObj);
                });

            FieldAsync<BooleanGraphType>(
                "deleteRequest",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> {Name = "id"}
                ),
                resolve: async context =>
                {
                    HttpStatusCode isValidAuthUser = await vtexApiService.IsValidAuthUser();

                    if (isValidAuthUser != HttpStatusCode.OK)
                    {
                        context.Errors.Add(new ExecutionError(isValidAuthUser.ToString())
                        {
                            Code = isValidAuthUser.ToString()
                        });

                        return default;
                    }

                    var id = context.GetArgument<string>("id");

                    return await availabilityRepository.DeleteNotifyRequest(id);
                });
            
            FieldAsync<ListGraphType<ProcessingResultType>>(
                "processUnsentRequests",
                resolve: async context =>
                {
                    HttpStatusCode isValidAuthUser = await vtexApiService.IsValidAuthUser();

                    if (isValidAuthUser != HttpStatusCode.OK)
                    {
                        context.Errors.Add(new ExecutionError(isValidAuthUser.ToString())
                        {
                            Code = isValidAuthUser.ToString()
                        });

                        return default;
                    }

                    return await vtexApiService.ProcessUnsentRequests();
                });
        }
    }
}
