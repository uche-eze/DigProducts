using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalProductAPI.Entities
{
    public class Bond
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Security Id is a required field")]
        public string SecurityID { get; set; }
        [Required(ErrorMessage = "Maturity value is a required field")]
        public string MaturityValue { get; set; }
        [Required(ErrorMessage = "Maturity date is a required field")]
        public string MaturityDate { get; set; }
        [Required(ErrorMessage = "Rate is a required field")]
        public decimal Rate { get; set; }
        [Required(ErrorMessage = "Currency is a required field")]
        public string Currency { get; set; }
        [Required(ErrorMessage = "Bid price is a required field")]
        public decimal BidPrice { get; set; }
        [Required(ErrorMessage = "Available volume is a required field")]
        public decimal AvailableVolume { get; set; }
        [Required(ErrorMessage = "Offer price is a required field")]
        public decimal OfferPrice { get; set; }
        public decimal Consideration { get; set; }
        public decimal FaceValue { get; set; }
        [Required(ErrorMessage = "Maker id is a required field")]//Add Portfolio ID in the properties
        public string MakerId { get; set; }
        public string AuthorizerId { get; set; }
        public DateTime AuthorizeDate { get; set; }
        public DateTime InputDate { get; set; } = DateTime.Now;
        public bool IsAuthorized { get; set; } = false;
        public string Status { get; set; } = "UNAUTHORIZED";
    }
}
