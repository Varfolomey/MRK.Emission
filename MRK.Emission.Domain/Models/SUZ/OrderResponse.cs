namespace MRK.Emission.Domain.Models.SUZ
{
    public class OrderResponse
    {
        public string omsId { get; set; }
        public string orderId { get; set; }
        public int expectedCompleteTimestamp { get; set; }
    }
}
