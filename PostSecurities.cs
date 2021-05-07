using DigitalProductAPI.RequestModels;
using DigitalProductAPI.ResponseModels;
using FCUBSSecuritiesService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalProductAPI
{
    public class PostSecurities
    {
        private static Random random = new Random();
        public static string GenerateBatchNo(int Range)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, Range).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public async Task<SecurityResponse> PostBuySell(SecurityRequest securityRequest, IConfiguration configuration, ILogger logger)
        {
            string productCode = "FSOP";
            string buyLeg = "BB";
            string sellLeg = "CS";
            string prdCode = "FSOP";
            string portfolio = "";
            string skLoc = "";
            string skAcc = "";
            string LgPrdCode = "FCSE";
            string cpty = "";
            string cptyFrom = securityRequest.CustomerId;
            string eventCode = "BOOK";
            string LgPrdCodeTo = "FBUY";
            string accomLodge = "Y";
            string skLocTo = "CBN";
            string skAccTo = "1216600";
            string portfolioTo = securityRequest.PortfolioId;
            string acmpWdrw = "N";
            string securityType = "Z";
            if (securityRequest.TransactionType.ToUpper() == "SELL")
            {
                productCode = "FSOS";
                buyLeg = "CB";
                sellLeg = "BS";
                prdCode = "FSOS";
                portfolio = securityRequest.PortfolioId;
                cptyFrom = null;
                skLoc = "CBN";
                skAcc = "1216600";
                LgPrdCode = "FSEL";
                LgPrdCodeTo = "FCBY";
                cpty = securityRequest.CustomerId;
                eventCode = "MSTL";
                accomLodge = null;
                skLocTo = null;
                skAccTo = null;
                portfolioTo = null;
                acmpWdrw = "Y";
            }
            if(securityRequest.SecurityType == "BOND")
            {
                securityType = "B";
            }
            SecurityResponse response = new SecurityResponse();

            var SecuritiesDeal = new CREATESECURITIESDEAL_FSFS_REQ()
            {
                FCUBS_HEADER = new FCUBS_HEADERType
                {
                    SOURCE = "TMSSOURCE",
                    UBSCOMP = UBSCOMPType.FCUBS,
                    USERID = securityRequest.UserId,
                    ENTITY = "ENTITY_ID1",
                    BRANCH = "000",
                    SERVICE = "FCUBSSecuritiesService",
                    OPERATION = "CreateSecuritiesDeal",
                },
                FCUBS_BODY = new CREATESECURITIESDEAL_FSFS_REQFCUBS_BODY
                {
                    DealmasterFull = new SecuritiesDealFullType
                    {
                        FACEDEAL = securityRequest.FaceValue,
                        TRADEDT = new DateTime(2020, 05, 29),//change to take todays date during GO-LIVE
                        AUTOMANDSTL = "A",
                        DSTLCOMPLETED = "N",
                        SE_TXNDATE = new DateTime(2020, 05, 29),//change to take todays date during GO-LIVE
                        SPOTPRCQT = "PRI",//get this value from the appsetting.json file,
                        SECSTTLMENT = "CBN",// get this value from appsetting.json file
                        ISMAIND = "N",
                        LIMITSTRACKINGREQD = "N",
                        UNITS = securityRequest.FaceValue,
                        EXERCISEFLAG = "N",
                        DLVRFREEPAY = "F",
                        RECVFREEPAY = "F",
                        REMARKS = securityRequest.Narration,
                        PRD = productCode,
                        BRN = "000",
                        BUYLEG = buyLeg,
                        SELLLEG = sellLeg,
                        VERSION = 1,
                        VERSIONSpecified = true,
                        SECTYCD = securityRequest.SecurityCode,
                        SECURITYTYPE = securityType,//B for bond and Z for TBills
                        DCONREQUIRED = "Y",
                        MKTCD = "FMDA",//get this value from the appsetting.json file
                        MATURITY_DATE = new DateTime(2020, 10, 22),
                        DSTLDATE = new DateTime(2020, 05, 29),
                        DSTLDATESpecified = true,
                        DEALTYPE = "S",
                        DEALTYP = "S",
                        QUOTEBY = "N",
                        DEAL_QUANTITY = securityRequest.FaceValue,
                        FACEVALPERUNITSpecified = true,
                        PRCQUOTE = "PRI",//get this value from the appsetting.json file
                        INPUTPRC = securityRequest.Rate,
                        PAYCCY = securityRequest.Currency,
                        RATEPCYSCY = 1,
                        RATEPCYSCYSpecified = true,
                        SCCY = securityRequest.Currency,
                        LATVERNO = "1",
                        FTYPE = "D",
                        Detalfrom = new SecuritiesDealFullTypeDetalfrom
                        {
                            BRANCH = "000",
                            LGPRDCODE = LgPrdCode,
                            EVENTCODE = "BOOK",
                            AUTOMSTL = "A",
                            MSTLCOMPLETED = "N",
                            PORTFOLIO = portfolio,
                            CPTY = cptyFrom,
                            PCY = securityRequest.Currency,
                            ACCOMLODGE = accomLodge,
                            PAYBRN = securityRequest.BranchCode,
                            PAYACC = securityRequest.AccountNumber,
                            SKLOC = skLoc,
                            SKACC = skAcc,
                            FRMTYPFROM = "D",
                            BRKPKPDONE = "N",
                            CHARGEPKPDONE = "N",
                            INTREFERDONE = "N",
                            SETTLEPICKUPDONE = "Y"
                        },
                        Detalto = new SecuritiesDealFullTypeDetalto
                        {
                            BRANCH = "000",
                            LGPRDCODE = LgPrdCodeTo,
                            VERSION = 1,
                            VERSIONSpecified = true,
                            EVENTCODE = eventCode,
                            AUTOMSTL = "A",
                            MSTLCOMPLETED = "N",
                            PORTFOLIO = portfolioTo,
                            CPTY = cpty,
                            PCY = securityRequest.Currency,
                            SKLOCTO = skLocTo,
                            SKACCTO = skAccTo,
                            ACMP_WDRW = acmpWdrw,
                            FRMTYPTO = "D",
                            BRKPKPDONE = "Y",
                            CHARGEPKPDONE = "Y",
                            INTREFERDONE = "Y",
                            SETTLEPICKUPDONE = "Y",
                            TAXPICKUPDONE = "Y",
                            MISPICKUPDONE = "Y",
                            PARTYPROCESSED = "Y",
                            ADVPICKUPDONE = "Y",
                            NETCONSPROCESSED = "N"
                        },
                        Contract = new SecuritiesDealFullTypeContract
                        {
                            LATVERNO = 1,
                            LATVERNOSpecified = true,
                            PRDCODE = prdCode,
                            MDCODE = "SD",
                            UDSTAT = "NORM",
                            CONTCCY = securityRequest.Currency,
                            XREF = GenerateBatchNo(10),
                        }
                    }
                }
            };
            FCUBSSecuritiesServiceSEIClient.EndpointConfiguration config = new FCUBSSecuritiesServiceSEIClient.EndpointConfiguration();
            var client = new FCUBSSecuritiesServiceSEIClient(config);

            try
            {
                var returnValue = await client.CreateSecuritiesDealFSAsync(SecuritiesDeal);
                if(returnValue.CREATESECURITIESDEAL_FSFS_RES != null)
                 {
                    if(returnValue.CREATESECURITIESDEAL_FSFS_RES.FCUBS_HEADER.MSGSTAT.ToString().ToLower() == "failure")
                    {                        
                        if (returnValue.CREATESECURITIESDEAL_FSFS_RES.FCUBS_BODY.FCUBS_WARNING_RESP != null)
                        {
                            List<string> warnings = new List<string>();
                            foreach (var item in returnValue.CREATESECURITIESDEAL_FSFS_RES.FCUBS_BODY.FCUBS_WARNING_RESP)
                            {
                                warnings.Add($"{item.WCODE} {item.WDESC}");
                            }
                            response.Warnings = warnings;
                        }
                        if (returnValue.CREATESECURITIESDEAL_FSFS_RES.FCUBS_BODY.FCUBS_ERROR_RESP != null)
                        {
                            List<String> errors = new List<string>();
                            foreach (var item in returnValue.CREATESECURITIESDEAL_FSFS_RES.FCUBS_BODY.FCUBS_ERROR_RESP)
                            {
                                errors.Add($"{item.ECODE} {item.EDESC}");
                            }
                            response.Errors = errors;
                        }
                        response.ResponseCode = 400;
                        response.Message = "Failure";
                        //logger.Information($"Post securities {ExternalReference} response - {response.ToString()}");
                        return response;
                    }
                    else
                    {
                        if (returnValue.CREATESECURITIESDEAL_FSFS_RES.FCUBS_HEADER.MSGSTAT.ToString().ToLower() == "success")
                        {
                            if (returnValue.CREATESECURITIESDEAL_FSFS_RES.FCUBS_BODY.FCUBS_WARNING_RESP != null)
                            {
                                List<string> warnings = new List<string>();
                                foreach (var item in returnValue.CREATESECURITIESDEAL_FSFS_RES.FCUBS_BODY.FCUBS_WARNING_RESP)
                                {
                                    warnings.Add($"{item.WCODE}{item.WDESC}");
                                }
                                response.Warnings = warnings;
                            }
                            if (returnValue.CREATESECURITIESDEAL_FSFS_RES.FCUBS_BODY.FCUBS_ERROR_RESP != null)
                            {
                                List<string> errors = new List<string>();
                                foreach (var item in returnValue.CREATESECURITIESDEAL_FSFS_RES.FCUBS_BODY.FCUBS_ERROR_RESP)
                                {
                                    errors.Add($"{item.ECODE} {item.EDESC}");
                                }
                                response.Errors = errors;
                            }
                            response.ResponseCode = 0;
                            response.Message = "Success";
                            //logger.Information($"Post securities {ExternalReference} response - {response.ToString()}");
                            return response;
                        }
                    }
                }
                response.Message = "server error";
                response.ResponseCode = 500;

                return response;
            }
            catch (Exception)
            {

                throw;
            }
        } 
    }
}
