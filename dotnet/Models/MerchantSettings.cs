using System;
using System.Collections.Generic;
using System.Text;

namespace AvailabilityNotify.Models
{
    public class MerchantSettings
    {
        public string AppKey { get; set; }
        public string AppToken { get; set; }
        public bool Initialized { get; set; }
        public bool DoShippingSim { get; set; }
    }
}
