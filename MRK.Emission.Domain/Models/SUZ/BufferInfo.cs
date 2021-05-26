using System;
using System.Collections.Generic;

namespace MRK.Emission.Domain.Models.SUZ
{
    /// <summary>
    /// https://xn--80ajghhoc2aj1c8b.xn--p1ai/upload/iblock/07f/STANTSIYA-UPRAVLENIYA-ZAKAZAMI.pdf
    /// 2.2.7.2 Ответ на запрос
    /// 
    /// Объект ответа на запрос о состоянии КМ в заказе.
    /// </summary>
    public class BufferInfo
    {
        public int availableCodes { get; set; }
        public string bufferStatus { get; set; }
        public string gtin { get; set; }
        public int leftInBuffer { get; set; }
        public string omsId { get; set; }
        public Guid orderId { get; set; }
        public List<PoolInfo> poolInfos { get; set; }
        public bool poolsExhausted { get; set; }
        public string rejectionReason { get; set; }
        public int totalCodes { get; set; }
        public int totalPassed { get; set; }
        public int unavailableCodes { get; set; }

    }
}
