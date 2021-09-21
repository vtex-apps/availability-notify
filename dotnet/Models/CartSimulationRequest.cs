using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvailabilityNotify.Models
{
    public class CartSimulationRequest
    {
        [JsonProperty("items")]
        public List<CartItem> Items { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }

    public class CartItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("requestIndex")]
        public long RequestIndex { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("seller")]
        public string Seller { get; set; }

        [JsonProperty("sellerChain")]
        public string[] SellerChain { get; set; }

        [JsonProperty("tax")]
        public long Tax { get; set; }

        [JsonProperty("priceValidUntil")]
        public DateTimeOffset PriceValidUntil { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("listPrice")]
        public long ListPrice { get; set; }

        [JsonProperty("rewardValue")]
        public long RewardValue { get; set; }

        [JsonProperty("sellingPrice")]
        public long SellingPrice { get; set; }

        [JsonProperty("offerings")]
        public object[] Offerings { get; set; }

        [JsonProperty("priceTags")]
        public object[] PriceTags { get; set; }

        [JsonProperty("measurementUnit")]
        public string MeasurementUnit { get; set; }

        [JsonProperty("unitMultiplier")]
        public long UnitMultiplier { get; set; }

        [JsonProperty("parentItemIndex")]
        public object ParentItemIndex { get; set; }

        [JsonProperty("parentAssemblyBinding")]
        public object ParentAssemblyBinding { get; set; }

        [JsonProperty("availability")]
        public string Availability { get; set; }
    }
}


