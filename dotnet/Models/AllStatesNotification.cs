using Newtonsoft.Json;
using System;

namespace AvailabilityNotify.Models
{
    public class AllStatesNotification
    {
        [JsonProperty("recorder")]
        public Recorder Recorder { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("currentState")]
        public string CurrentState { get; set; }

        [JsonProperty("lastState")]
        public string LastState { get; set; }

        [JsonProperty("currentChangeDate")]
        public DateTimeOffset CurrentChangeDate { get; set; }

        [JsonProperty("lastChangeDate")]
        public DateTimeOffset LastChangeDate { get; set; }
    }

    public class Recorder
    {
        [JsonProperty("_record")]
        public Record Record { get; set; }
    }

    public class Record
    {
        [JsonProperty("x-vtex-meta")]
        public XVtexMeta XVtexMeta { get; set; }

        [JsonProperty("x-vtex-meta-bucket")]
        public XVtexMeta XVtexMetaBucket { get; set; }
    }

    public class XVtexMeta
    {
    }
}
