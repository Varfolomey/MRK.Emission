namespace MRK.Emission.Domain.Models.SUZ
{
    public class PoolInfo
    {
        public bool isRegistrarReady { get; set; }
        public long lastRegistrarErrorTimestamp { get; set; }
        public int leftInRgistrar { get; set; }
        public int quantity { get; set; }
        public int registrarErrorCount { get; set; }
        public string registrarId { get; set; }
        public string rejectionReason { get; set; }
        public string status { get; set; }
    }
}
