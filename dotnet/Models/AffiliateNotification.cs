using Newtonsoft.Json;

namespace AvailabilityNotify.Models
{
    public partial class AffiliateNotification
    {
        [JsonProperty("IdSku")]
        public string IdSku { get; set; }

        [JsonProperty("ProductId")]
        public long ProductId { get; set; }

        [JsonProperty("An")]
        public string An { get; set; }

        [JsonProperty("IdAffiliate")]
        public string IdAffiliate { get; set; }

        [JsonProperty("Version")]
        public string Version { get; set; }

        [JsonProperty("DateModified")]
        public string DateModified { get; set; }

        [JsonProperty("IsActive")]
        public bool IsActive { get; set; }

        [JsonProperty("StockModified")]
        public bool StockModified { get; set; }

        [JsonProperty("PriceModified")]
        public bool PriceModified { get; set; }

        [JsonProperty("HasStockKeepingUnitModified")]
        public bool HasStockKeepingUnitModified { get; set; }

        [JsonProperty("HasStockKeepingUnitRemovedFromAffiliate")]
        public bool HasStockKeepingUnitRemovedFromAffiliate { get; set; }
    }
}
