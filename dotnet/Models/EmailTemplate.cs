using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvailabilityNotify.Models
{
    public class EmailTemplate
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("FriendlyName")]
        public string FriendlyName { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("IsDefaultTemplate")]
        public bool IsDefaultTemplate { get; set; }

        [JsonProperty("AccountId")]
        public object AccountId { get; set; }

        [JsonProperty("AccountName")]
        public object AccountName { get; set; }

        [JsonProperty("ApplicationId")]
        public object ApplicationId { get; set; }

        [JsonProperty("IsPersisted")]
        public bool IsPersisted { get; set; }

        [JsonProperty("IsRemoved")]
        public bool IsRemoved { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Templates")]
        public Templates Templates { get; set; }
    }

    public class Templates
    {
        [JsonProperty("email")]
        public Email Email { get; set; }

        [JsonProperty("sms")]
        public Sms Sms { get; set; }
    }

    public class Email
    {
        [JsonProperty("To")]
        public string To { get; set; }

        [JsonProperty("CC")]
        public object Cc { get; set; }

        [JsonProperty("BCC")]
        public object Bcc { get; set; }

        [JsonProperty("Subject")]
        public string Subject { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("ProviderId")]
        public object ProviderId { get; set; }

        [JsonProperty("ProviderName")]
        public object ProviderName { get; set; }

        [JsonProperty("IsActive")]
        public bool IsActive { get; set; }

        [JsonProperty("withError")]
        public bool WithError { get; set; }
    }

    public class Sms
    {
        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("ProviderId")]
        public object ProviderId { get; set; }

        [JsonProperty("ProviderName")]
        public object ProviderName { get; set; }

        [JsonProperty("IsActive")]
        public bool IsActive { get; set; }

        [JsonProperty("withError")]
        public bool WithError { get; set; }

        [JsonProperty("Parameters")]
        public List<object> Parameters { get; set; }
    }
}
