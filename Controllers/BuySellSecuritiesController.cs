using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using DigitalProductAPI.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DigitalProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class BuySellSecuritiesController : ControllerBase
    {
        public BuySellSecuritiesController(IConfiguration configuration, ILogger<BuySellSecuritiesController> logger)
        {
            Configuration = configuration;
            Logger = logger;
        }

        public IConfiguration Configuration { get; }
        public ILogger<BuySellSecuritiesController> Logger { get; }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] SecurityRequest securityRequest)
        {
            Logger.LogInformation($"Save Bonds request ::: {JsonConvert.SerializeObject(securityRequest)}");
            //if (securityRequest.SecurityType.ToUpper() != "TBILL" || securityRequest.SecurityType.ToUpper() != "BOND") return BadRequest("please pass TBILL or BOND for security type");
            //if (securityRequest.TransactionType.ToUpper() != "BUY" || securityRequest.SecurityType.ToUpper() != "SELL") return BadRequest("please pass BUY or SELL for Transaction Type");
            var postSecurity = new PostSecurities();
            try
            {
                var response = await postSecurity.PostBuySell(securityRequest, Configuration, Logger);
                if (response.ResponseCode == 0)
                {
                    return Ok(response);
                }else if(response.ResponseCode == 400)
                {
                    return BadRequest(response);
                }
                else
                {
                    return StatusCode(500);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"unable to Purchase/Sell security ::: {JsonConvert.SerializeObject(Response)}");
                return StatusCode(500);
            }            
        }
    }
}
