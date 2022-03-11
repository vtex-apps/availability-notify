using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvailabilityNotify.Models
{
    public class GetSkuSellerResponse
    {
        [JsonProperty("StockKeepingUnitId")]
        public int StockKeepingUnitId { get; set; }
    }
}
