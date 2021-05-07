using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace DigitalProductAPI.RequestModels
{
    public class SecurityRequest
    {
        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; }
        [Required(ErrorMessage ="FaceValue is required")]
        public decimal FaceValue { get; set; }
        [Required(ErrorMessage ="Trade Date is required")]
        public DateTime TradeDate { get; set; }
        //[Required(ErrorMessage = "Security Type is required (please enter TBILL or BOND)")]
        public string SecurityType { get; set; }
        public string Narration { get; set; } = "";
        [Required(ErrorMessage ="Transaction Type is required(Please pass in either BUY or SELL)")]
        public string TransactionType { get; set; }
        [Required(ErrorMessage ="Security Code is required")]
        public string SecurityCode { get; set; }
        [Required(ErrorMessage ="Rate is required")]
        public decimal Rate { get; set; }
        [Required(ErrorMessage ="Currency is required, please pass the currency of the secuity to be transacted")]
        public string Currency { get; set; }
        [Required(ErrorMessage ="Portfolio Id is required, Please pass the portfolio of the security to be transacted")]
        public string PortfolioId { get; set; }
        [Required(ErrorMessage ="customer Id is required, Please pass the Id of the transacting customer")]
        public string CustomerId { get; set; }
        [Required(ErrorMessage ="branch code is required, please pass the customer account branch")]
        public string BranchCode { get; set; }
        [Required(ErrorMessage ="Account number is required, please pass the account number of the customer transacting")]
        public string AccountNumber { get; set; }
    }
}
