using System.Collections.Generic;

namespace MRK.Emission.Domain.Models.SUZ
{
    public class Order
    {
        public string contactPerson { get { return "ECCO"; } }
        public string releaseMethodType { get { return "IMPORT"; } }
        public string createMethodType { get { return "SELF_MADE"; } }
        public bool remainsAvailable { get { return false; } }
        public bool remainsImport { get { return false; } }
        public List<OrderProduct> products { get; set; }

    }
}
