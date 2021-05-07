using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalProductAPI.ResponseModels
{
    public class CustomerPortfolio
    {
        public string BranchCode { get; set; }
        public string CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public string SecurityCurrency { get; set; }
        public string SecurityId { get; set; }
        public string ProductCode { get; set; }
        public string SecurityType { get; set; }
        public string PortfolioProduct { get; set; }
        public decimal Quantity { get; set; }
    }
}
