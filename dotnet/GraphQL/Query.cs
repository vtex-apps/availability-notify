using System.Collections.Generic;
using AvailabilityNotify.GraphQL.Types;
using AvailabilityNotify.Models;
using AvailabilityNotify.Services;
using GraphQL;
using GraphQL.Types;
using Vtex.Api.Context;

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
                    NotifyRequest[] notifyRequests = await availabilityRepository.ListNotifyRequests();
                    List<NotifyRequest> requestList = new List<NotifyRequest>(notifyRequests);
                    return requestList;
                }
            );
        }
    }
}