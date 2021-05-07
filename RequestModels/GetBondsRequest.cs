using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalProductAPI.RequestModels
{
    public class GetBondsRequest
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        [Required(ErrorMessage = "IsDefault is a required field")]
        public bool IsDefault { get; set; }
    }
}
