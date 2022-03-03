using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvailabilityNotify.Models
{
    public class Lock
    {
        [JsonProperty("processing_started")]
        public DateTime ProcessingStarted { get; set; }
    }
}
