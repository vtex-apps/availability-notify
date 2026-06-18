using AvailabilityNotify.GraphQL.Types;
using AvailabilityNotify.Models;
using AvailabilityNotify.Services;
using GraphQL;
using GraphQL.Types;
using Vtex.Api.Context;
using System.Net;

using System;

namespace AvailabilityNotify.GraphQL
{
    [GraphQLMetadata("Mutation")]
    public class Mutation : ObjectGraphType<object>
    {
        public Mutation(IIOServiceContext contextService, IVtexAPIService vtexApiService, IAvailabilityRepository availabilityRepository)
        {
            Name = "Mutation";

            FieldAsync<BooleanGraphType>(
                "availabilitySubscribe",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> {Name = "name"},
                    new QueryArgument<StringGraphType> {Name = "email"},
                    new QueryArgument<StringGraphType> {Name = "skuId"},
                    new QueryArgument<StringGraphType> {Name = "locale"},
                    new QueryArgument<StringGraphType> {Name = "saleChannel"},
                    new QueryArgument<NonNullGraphType<SellerObjInputType>> {Name = "sellerObj"}
                ),
                resolve: async context =>
                {
                    var name = context.GetArgument<string>("name");
                    var email = context.GetArgument<string>("email");
                    var skuId = context.GetArgument<string>("skuId");
                    var locale = context.GetArgument<string>("locale");
                    var saleChannel = context.GetArgument<string>("saleChannel");
                    var sellerObj = context.GetArgument<SellerObj>("sellerObj");

                    Console.WriteLine("saleChannel");
                    Console.WriteLine(saleChannel);

                    if (saleChannel != "1" && saleChannel != null) {
                        Console.WriteLine("llegaaaaaaaaaaaa");
                        Console.WriteLine(saleChannel);
                        var sellerData = await vtexApiService.GetSellerName(saleChannel);
                        Console.WriteLine("sellerDataaaa");
                        Console.WriteLine(sellerData.Id);
                        Console.WriteLine(sellerData.Name);

                        contextService.Vtex.Logger.Debug("GraphQL", null, $"AvailabilitySubscribe Mutation called: '{name}' '{email}' '{skuId}' '{locale}'");

                        if(sellerData.Id != "" && sellerData.Name != "") {

                            SellerObj SellerObjBetterScope = new SellerObj
                            {
                                sellerId = sellerData.Id,
                                sellerName = sellerData.Name,
                                addToCartLink = sellerObj.addToCartLink,
                                sellerDefault = sellerObj.sellerDefault
                            };

                            Console.WriteLine("New Object SellerObjBetterScope");

                            Console.WriteLine(SellerObjBetterScope.sellerId);
                            Console.WriteLine(SellerObjBetterScope.sellerName);
                            Console.WriteLine(SellerObjBetterScope.addToCartLink);
                            Console.WriteLine(SellerObjBetterScope.sellerDefault);


                            return vtexApiService.AvailabilitySubscribe(email, skuId, name, locale, SellerObjBetterScope);
                            
                        } else {
                            return vtexApiService.AvailabilitySubscribe(email, skuId, name, locale, sellerObj);
                        }
                    } else {

                        Console.WriteLine("New Object sellerObj");

                        Console.WriteLine(sellerObj);
                        Console.WriteLine(sellerObj.sellerId);
                        Console.WriteLine(sellerObj.sellerName);
                        Console.WriteLine(sellerObj.addToCartLink);
                        Console.WriteLine(sellerObj.sellerDefault);

                        return vtexApiService.AvailabilitySubscribe(email, skuId, name, locale, sellerObj);
                    }
                    
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
