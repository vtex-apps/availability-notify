namespace AvailabilityNotify.Models
{
    public class ProcessingResult
    {
        public string SkuId { get; set; }
        public string QuantityAvailable { get; set; }
        public string Email { get; set; }
        public bool Sent { get; set; }
        public bool Updated { get; set; }
    }
}
