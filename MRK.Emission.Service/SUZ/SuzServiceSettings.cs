namespace MRK.Emission.Service.SUZ
{
    public class SuzServiceSettings
    {
        public string Host { get; set; } = "https://int01.gismt.crpt.tech/api/v3/true-api";
        public int Interval { get; set; }
        public string SertNum { get; set; }
        public string OmsConnectionId { get; set; }
        public string SuzClientToken { get; set; }
        public string EmissionUrl { get; set; }
        public string SuzOmsId { get; set; }
    }
}
