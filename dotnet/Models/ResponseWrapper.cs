namespace AvailabilityNotify.Models
{
    public class ResponseWrapper
    {
        public string ResponseText { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public string MasterDataToken { get; set; }
        public string Total { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
