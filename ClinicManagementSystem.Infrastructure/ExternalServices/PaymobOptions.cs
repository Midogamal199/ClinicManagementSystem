namespace ClinicManagementSystem.Infrastructure.ExternalServices
{
    public class PaymobOptions
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
        public string IntegrationId { get; set; }
        public string IframeId { get; set; }
        public string HmacSecret { get; set; }
        public string Currency { get; set; }
    }
}