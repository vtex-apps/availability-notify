using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvailabilityNotify.Models
{
    public class ShopperAddress
{
        [JsonProperty("addressName")]
        public string AddressName { get; set; }

        [JsonProperty("addressType")]
        public string AddressType { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("complement")]
        public object Complement { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("countryfake")]
        public object Countryfake { get; set; }

        [JsonProperty("geoCoordinate")]
        public double[] GeoCoordinate { get; set; }

        [JsonProperty("neighborhood")]
        public object Neighborhood { get; set; }

        [JsonProperty("number")]
        public object Number { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("receiverName")]
        public string ReceiverName { get; set; }

        [JsonProperty("reference")]
        public object Reference { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("accountId")]
        public Guid AccountId { get; set; }

        [JsonProperty("accountName")]
        public string AccountName { get; set; }

        [JsonProperty("dataEntityId")]
        public string DataEntityId { get; set; }

        [JsonProperty("createdBy")]
        public Guid CreatedBy { get; set; }

        [JsonProperty("createdIn")]
        public DateTimeOffset CreatedIn { get; set; }

        [JsonProperty("updatedBy")]
        public Guid? UpdatedBy { get; set; }

        [JsonProperty("updatedIn")]
        public DateTimeOffset? UpdatedIn { get; set; }

        [JsonProperty("lastInteractionBy")]
        public Guid LastInteractionBy { get; set; }

        [JsonProperty("lastInteractionIn")]
        public DateTimeOffset LastInteractionIn { get; set; }

        [JsonProperty("followers")]
        public object[] Followers { get; set; }

        [JsonProperty("tags")]
        public object[] Tags { get; set; }

        [JsonProperty("auto_filter")]
        public object AutoFilter { get; set; }
    }
}
