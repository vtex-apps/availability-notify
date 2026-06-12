using System.Collections.Generic;
using Newtonsoft.Json;

namespace AvailabilityNotify.Models
{
    public class LmRole
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("resources")]
        public List<LmResource> Resources { get; set; }
    }

    public class LmResource
    {
        [JsonProperty("key")]
        public string Key { get; set; }
    }
}
