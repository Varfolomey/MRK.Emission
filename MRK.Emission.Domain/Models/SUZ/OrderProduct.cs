namespace MRK.Emission.Domain.Models.SUZ
{
    public class OrderProduct
    {
        public string gtin { get; set; }
        public int quantity { get; set; }

        public string serialNumberType { get { return "OPERATOR"; } }

        public int templateId { get { return 1; } }

    }
}
