
namespace Monopolizers.Common.DTO
{
    public class PayOSWebhookInput
    {
        public string Code { get; set; }
        public string Desc { get; set; }
        public bool Success { get; set; }
        public PayOSWebhookDTO Data { get; set; }
        public string Signature { get; set; }
    }
}
