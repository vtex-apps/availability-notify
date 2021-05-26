using System;
using System.Collections.Generic;
using System.Text;

namespace AvailabilityNotify.Models
{
    public class JsonData
    {
        public GetSkuContextResponse SkuContext { get; set; }
        public NotifyRequest NotifyRequest { get; set; }
    }

    public class EmailMessage
    {
        public object ProviderName { get; set; }
        public string TemplateName { get; set; }
        public JsonData JsonData { get; set; }
    }
}
