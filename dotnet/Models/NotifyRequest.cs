using System;
using Newtonsoft.Json;

namespace AvailabilityNotify.Models
{
    public class NotifyRequest
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("skuId")]
        public string SkuId { get; set; }

        [JsonProperty("createdAt")]
        public string RequestedAt { get; set; }

        [JsonProperty("notificationSend")]
        public string NotificationSent { get; set; }

        [JsonProperty("sendAt", NullValueHandling = NullValueHandling.Ignore)]
        public string NotificationSentAt { get; set; }

        [JsonProperty("locale", NullValueHandling = NullValueHandling.Ignore)]
        public string Locale { get; set; }

        [JsonProperty("seller", NullValueHandling = NullValueHandling.Ignore)]
        public SellerObj Seller { get; set; }
    }
}
