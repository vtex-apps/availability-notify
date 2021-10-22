using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvailabilityNotify.Models
{
    public class ListAllWarehousesResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("warehouseDocks")]
        public List<WarehouseDock> WarehouseDocks { get; set; }

        [JsonProperty("pickupPointIds")]
        public object[] PickupPointIds { get; set; }

        [JsonProperty("priority")]
        public long Priority { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }

    public class WarehouseDock
    {
        [JsonProperty("dockId")]
        public string DockId { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("cost")]
        public long Cost { get; set; }
    }
}
