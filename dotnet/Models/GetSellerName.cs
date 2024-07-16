using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvailabilityNotify.Models
{
    public class GetSellerNameResponse
    {
        [JsonProperty("paging")]
        public Paging Paging { get; set; }

        [JsonProperty("items")]
        public Items[] Items { get; set; }

    }

    public partial class Paging
    {
        [JsonProperty("from")]
        public long? From { get; set; }

        [JsonProperty("to")]
        public long? To { get; set; }

        [JsonProperty("total")]
        public long? Total { get; set; }
    }

    public partial class Items
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("logo")]
        public string? Logo { get; set; }

        [JsonProperty("taxCode")]
        public string? TaxCode { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("sellerCommissionConfiguration")]
        public string? SellerCommissionConfiguration { get; set; }

        [JsonProperty("catalogSystemEndpoint")]
        public string? CatalogSystemEndpoint { get; set; }

        [JsonProperty("CSCIdentification")]
        public string? CSCIdentification { get; set; }

        [JsonProperty("account")]
        public string? Account { get; set; }

        [JsonProperty("channel")]
        public string? Channel { get; set; }

        [JsonProperty("salesChannel")]
        public string? SalesChannel { get; set; }

        [JsonProperty("score")]
        public long? Score { get; set; }

        [JsonProperty("exchangeReturnPolicy")]
        public string? ExchangeReturnPolicy { get; set; }

        [JsonProperty("deliveryPolicy")]
        public string? DeliveryPolicy { get; set; }

        [JsonProperty("securityPrivacyPolicy")]
        public string? SecurityPrivacyPolicy { get; set; }

        [JsonProperty("fulfillmentSellerId")]
        public string FulfillmentSellerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("allowHybridPayments")]
        public bool AllowHybridPayments { get; set; }

        [JsonProperty("isBetterScope")]
        public bool IsBetterScope { get; set; }

        [JsonProperty("sellerType")]
        public long SellerType { get; set; }

        [JsonProperty("availableSalesChannels")]
        public AvailableSalesChannels[] AvailableSalesChannels { get; set; }

        [JsonProperty("isVtex")]
        public bool IsVtex { get; set; }

        [JsonProperty("trustPolicy")]
        public string TrustPolicy { get; set; }

        [JsonProperty("policies")]
        public string[] Policies { get; set; }

    }

    public partial class AvailableSalesChannels
    {
        [JsonProperty("id")]
        public long? Id { get; set; }
    }
    
}
