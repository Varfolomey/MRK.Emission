using MRK.Emission.Domain.Enums;
using MRK.Emission.Domain.Models.SUZ;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MRK.Emission.Domain.Models
{
    [Table("ORDERDOCUMENTLINE")]
    public class OrderDocumentLine
    {
        public string documentId { get; set; }

        [Required(ErrorMessage = "Поле \"DocumentLineNum\" должно быть заполнено.")]
        public int documentLineNum { get; set; }

        [Required(ErrorMessage = "Поле \"Gtin\" должно быть заполнено.")]
        public string gtin { get; set; }

        [Required(ErrorMessage = "Поле \"Qty\" должно быть заполнено.")]
        public int qty { get; set; }
        public string orderId { get; set; }
        public DocumentLineStatus documentLineStatus { get; set; }
        public DateTime orderDate { get; set; }
        public string clientName { get; set; }
        public string lastBlockId { get; set; }
        [NotMapped]
        public OrderDocument orderDocument { get; set; }

        [NotMapped]
        public OrderResponse orderResponse { get; set; }
    }
}
