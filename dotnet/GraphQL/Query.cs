using System.Collections.Generic;
using AvailabilityNotify.GraphQL.Types;
using AvailabilityNotify.Models;
using AvailabilityNotify.Services;
using GraphQL;
using GraphQL.Types;
using Vtex.Api.Context;
using System.Net;

namespace AvailabilityNotify.GraphQL
{
    [GraphQLMetadata("Query")]
    public class Query : ObjectGraphType<object>
    {
        public Query(IIOServiceContext context, IVtexAPIService vtexApiService, IAvailabilityRepository availabilityRepository)
        {
            Name = "Query";

            FieldAsync<ListGraphType<NotifyRequestType>>(
                "listRequests",
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

                    NotifyRequest[] notifyRequests = await availabilityRepository.ListNotifyRequests();
                    List<NotifyRequest> requestList = new List<NotifyRequest>(notifyRequests);

                    return requestList;
                }
            );
        }
    }
}