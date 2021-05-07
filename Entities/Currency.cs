using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalProductAPI.Entities
{
    public class Currency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Currency Code is a required field")]
        public string CurrencyCode { get; set; }
        [Required(ErrorMessage = "Currency Description is a required field")]
        public string CurrencyDescription { get; set; }
        public DateTime SetUpDate { get; set; } = DateTime.Now;
    }
}
