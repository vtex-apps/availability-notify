using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvailabilityNotify.Models
{
    public class SlaRequest
    {
        [JsonProperty("items")]
        public Item[] Items { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("salesChannel")]
        public string SalesChannel { get; set; }
    }

    public class Item
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("groupItemId")]
        public object GroupItemId { get; set; }

        [JsonProperty("kitItem")]
        public object[] KitItem { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("additionalHandlingTime")]
        public DateTimeOffset AdditionalHandlingTime { get; set; }

        [JsonProperty("dimension")]
        public Dimension Dimension { get; set; }
    }

    public class Location
    {
        [JsonProperty("zipCode")]
        public string ZipCode { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("instore")]
        public Instore Instore { get; set; }
    }

    public class Instore
    {
        [JsonProperty("isCheckedIn")]
        public bool IsCheckedIn { get; set; }

        [JsonProperty("storeId")]
        public string StoreId { get; set; }
    }
}

