using AvailabilityNotify.Models;
using GraphQL;
using GraphQL.Types;

        // public string SkuId { get; set; }
        // public string QuantityAvailable { get; set; }
        // public string Email { get; set; }
        // public bool Sent { get; set; }
        // public bool Updated { get; set; }

namespace AvailabilityNotify.GraphQL.Types
{
    [GraphQLMetadata("ProcessingResult")]
    public class ProcessingResultType : ObjectGraphType<ProcessingResult>
    {
        public ProcessingResultType()
        {
            Name = "ProcessingResult";
            Field(c => c.SkuId, type: typeof(StringGraphType)).Description("SkuId");
            Field(c => c.QuantityAvailable, type: typeof(StringGraphType)).Description("QuantityAvailable");
            Field(c => c.Sent, type: typeof(StringGraphType)).Description("Sent");
            Field(c => c.Email, type: typeof(StringGraphType)).Description("Email");
            Field(c => c.Updated, type: typeof(StringGraphType)).Description("Updated");
        }
    }
}
