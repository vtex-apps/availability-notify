using Newtonsoft.Json;

namespace AvailabilityNotify.Models
{
    public class InventoryBySku
    {
        [JsonProperty("skuId")]
        public string SkuId { get; set; }

        [JsonProperty("balance")]
        public Balance[] Balance { get; set; }
    }

    public class Balance
    {
        [JsonProperty("warehouseId")]
        public string WarehouseId { get; set; }

        [JsonProperty("warehouseName")]
        public string WarehouseName { get; set; }

        [JsonProperty("totalQuantity")]
        public long TotalQuantity { get; set; }

        [JsonProperty("reservedQuantity")]
        public long ReservedQuantity { get; set; }

        [JsonProperty("hasUnlimitedQuantity")]
        public bool HasUnlimitedQuantity { get; set; }

        [JsonProperty("timeToRefill")]
        public object TimeToRefill { get; set; }

        [JsonProperty("dateOfSupplyUtc")]
        public object DateOfSupplyUtc { get; set; }
    }
}