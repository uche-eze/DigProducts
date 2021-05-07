using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalProductAPI.RequestModels
{
    public class AuthorizeTreasuryBillRequest
    {
        [Required(ErrorMessage = "id is a required field")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Authorizer id is a required field")]
        public string AuthorizerId { get; set; }
    }
}
