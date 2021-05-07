using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalProductAPI.ResponseModels
{
    public class SecurityResponse
    {
        public List<string> Errors { get; set; }
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public List<string> Warnings { get; set; }
    }
}
