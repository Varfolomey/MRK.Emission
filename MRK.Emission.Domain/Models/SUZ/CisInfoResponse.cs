using System;

namespace MRK.Emission.Domain.Models.SUZ
{
    public class CisInfoResponse
    {
        public string uit { get; set; }
        public string cis { get; set; }
        public string gtin { get; set; }
        public string sgtin { get; set; }
        public string productName { get; set; }
        public DateTime emissionDate { get; set; }
        public string producerName { get; set; }
        public string producerInn { get; set; }
        public string lastDocId { get; set; }
        public string lastDocType { get; set; }
        public string emissionType { get; set; }
        public string status { get; set; }
        public string packType { get; set; }
        public int countChildren { get; set; }
        public DateTime introducedDate { get; set; }
        public string turnoverType { get; set; }
        public DateTime lastStatusChangeDate { get; set; }
        public string productGroup { get; set; }
    }
    
}
