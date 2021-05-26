using MRK.Emission.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MRK.Emission.Domain.Models
{
    [Table("ORDERDOCUMENTTABLE")]
    public class OrderDocument
    {
        public OrderDocument()
        {
            documentStatus = OrderDocumentStatus.CREATED;
        }

        [Required(ErrorMessage = "Поле \"DocumentId\" должно быть заполнено.")]
        public string documentId { get; set; }

        [Required(ErrorMessage = "Поле \"DocumentDate\" должно быть заполнено.")]
        public DateTime documentDate { get; set; }

        public CISReleaseType releaseType { get; set; }

        public string description { get; set; }

        public OrderDocumentStatus documentStatus { get; set; }
        public string clientName { get; set; }

        [NotMapped]
        public List<OrderDocumentLine> documentLines { get; set; }

    }
}
