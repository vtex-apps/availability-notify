using AvailabilityNotify.Models;
using GraphQL;
using GraphQL.Types;

namespace AvailabilityNotify.GraphQL.Types
{
    [GraphQLMetadata("NotifyRequest")]
    public class NotifyRequestType : ObjectGraphType<NotifyRequest>
    {
        public NotifyRequestType()
        {
            Name = "NotifyRequest";
            Field(c => c.Id, type: typeof(StringGraphType)).Description("Request Id");
            Field(c => c.Name, type: typeof(StringGraphType)).Description("Customer Name");
            Field(c => c.NotificationSent, type: typeof(StringGraphType)).Description("Was Notification Sent");
            Field(c => c.NotificationSentAt, type: typeof(StringGraphType)).Description("Notification Sent At");
            Field(c => c.RequestedAt, type: typeof(StringGraphType)).Description("Notification Was Requested At");
            Field(c => c.SkuId, type: typeof(StringGraphType)).Description("Sku Id");
            Field(c => c.Email, type: typeof(StringGraphType)).Description("Email");
        }
    }
}
