using DigitalProductAPI.ResponseModels;
using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace DigitalProductAPI
{
    public class CustomerSecurityPosition
    {
        public List<CustomerPortfolio> GetSecurityPositionByCustId(string customerId, string constring)
        {
            List<CustomerPortfolio> portfolios = new List<CustomerPortfolio>();
            using (OracleConnection conn = new OracleConnection(constring))
            {
                conn.Open();

                string SelectQuery = string.Format(@"select a.brn, a.cpty, b.customer_name1,a.sccy,C.internal_Sec_id, C.Product, c.security_type,
                                                    decode(C.product, 'CBND', 'CORPORATE BOND', 'FBND', 'FGN BOND', 'TBIL', 'TREASURY BILLS', 'CPIL', 'PROMISSORY NOTE', 'CBIL', 'COMMERCIAL BILL') AS Portfolio_Product,
                                                    sum(decode(substr(a.fccref, 4, 4), 'FSOP', A.DEALQTY, -A.DEALQTY)) dEAL_QTY
                                                    from fcjlive.vw_sevw_deal_sum a, 
                                                    fcjlive.sttm_customer b, fcjlive.SETM_SECURITY_MASTER c
                                                    where a.cpty is not null and a.contstat <> 'V'
                                                    and a.cpty = b.customer_no and a.product_code IN('FSOP','FSOS')
                                                    and c.internal_Sec_id = a.sectycd
                                                    and a.cpty = '{0}'
                                                    GROUP BY a.brn, a.cpty, b.customer_name1,a.sccy,C.internal_Sec_id, C.Product, c.security_type", customerId);

                OracleCommand myCommand = new OracleCommand(SelectQuery, conn);
                myCommand.CommandText = SelectQuery;
                myCommand.CommandType = CommandType.Text;
                OracleDataReader dr = myCommand.ExecuteReader();

                while (dr.Read())
                {
                    portfolios.Add(new CustomerPortfolio
                    {
                         BranchCode = dr.IsDBNull(0) ? "": dr.GetString(0),
                         CustomerNumber = dr.IsDBNull(1) ? "" : dr.GetString(1),
                         CustomerName = dr.IsDBNull(2) ? "" : dr.GetString(2),
                         SecurityCurrency = dr.IsDBNull(3) ? "" : dr.GetString(3),
                         SecurityId = dr.IsDBNull(4) ? "" : dr.GetString(4),
                         ProductCode = dr.IsDBNull(5) ? "" : dr.GetString(5),
                         SecurityType = dr.IsDBNull(6) ? "" : dr.GetString(6),
                         PortfolioProduct = dr.IsDBNull(7) ? "" : dr.GetString(7),
                         Quantity = dr.IsDBNull(8) ? 0 : dr.GetDecimal(8)
                    });
                }
                return portfolios;
            }                
        }
        
    }
}
