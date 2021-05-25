using System;
using System.Collections.Generic;
using System.Text;

namespace AvailabilityNotify.Models
{
    public class JsonData
    {
        
    }

    public class EmailMessage
    {
        public object providerName { get; set; }
        public string templateName { get; set; }
        public JsonData jsonData { get; set; }
    }
}
