using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalProductAPI.ResponseModels
{
    public class TBillsFileUploadResponse
    {
        public int Id { get; set; }        
        public string SecurityID { get; set; }
        public string MaturityValue { get; set; }
        public string MaturityDate { get; set; }
        public string Rate { get; set; }
        public string BidPrice { get; set; }
        public string AvailableVolume { get; set; }
        public string OfferPrice { get; set; }
        public string Consideration { get; set; }
        public string FaceValue { get; set; }
        public string Currency { get; set; }
    }
}
