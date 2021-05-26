using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MRK.Emission.Domain.Models.SUZ
{
    public class CisInfoParser
    {
        public CisInfoParser()
        {
            status = "";
            statusEx = "";
            productName = "";
            turnovertype = "";
            cis = "";
            ownerInn = "";
            ownerName = "";
        }
        /// <summary>
        /// EMITTED – Эмиттирован
        /// APPLIED – нанесён на товар
        /// INTRODUCED – можно продавать
        /// RETIRED – продан
        /// </summary>
        public string status { get; set; }
        public string statusEx { get; set; }
        public string turnovertype { get; set; }
        public string productName { get; set; }
        public string cis { get; set; }
        public string ownerInn { get; set; }
        public string ownerName { get; set; }
    }
}
