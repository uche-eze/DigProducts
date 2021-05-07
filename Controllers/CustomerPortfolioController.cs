using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DigitalProductAPI.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CustomerPortfolioController : ControllerBase
    {
        public CustomerPortfolioController(ILogger<CustomerPortfolioController> logger, IConfiguration configuration)
        {
            Logger = logger;
            Configuration = configuration;
        }

        public ILogger<CustomerPortfolioController> Logger { get; }
        public IConfiguration Configuration { get; }

        [Route("api/[controller]/GetPortfolioByCustomerId")]
        [HttpGet]
        public ActionResult GetPortfolioByCustId(string customerId)
        {
            Logger.LogInformation($"Request to get customer portfolio by customer Id::: customer Id = {customerId}");

            //string Config = Configuration["ConnectionString:tns"];
            string Config = $"User Id={Configuration["ConnectionString:userId"]}; Password={Configuration["ConnectionString:password"]}; Data Source = {Configuration["ConnectionString:tns"]};";
            if (string.IsNullOrWhiteSpace(customerId))
            {
                Logger.LogWarning("The request does not contain customer Id::: this is a compulsor field");
                ModelState.AddModelError("Request Error", "Please check that the request contains a Customer Id");
                return BadRequest(ModelState);
            }
            CustomerSecurityPosition customerPosition = new CustomerSecurityPosition();
            try
            {
                var position = customerPosition.GetSecurityPositionByCustId(customerId, Config);
                return Ok(position);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to query Flexcube for the customer position");
                return StatusCode(500);
            }
        }

        //[Route("api/[controller]/GetPortfolioByAccountNumber")]
        //[HttpGet]
        //public ActionResult GetPortfolioByAccountNo(string accountNumber)
        //{
        //    Logger.LogInformation($"Request to get customer portfolio by Account Number::: customer Id = {accountNumber}");

        //    //string Config = Configuration["ConnectionString:tns"];
        //    string Config = $"User Id={Configuration["ConnectionString:userId"]}; Password={Configuration["ConnectionString:password"]}; Data Source = {Configuration["ConnectionString:tns"]};";
        //    if (string.IsNullOrWhiteSpace(accountNumber))
        //    {
        //        Logger.LogWarning("The request does not contain customer Id::: this is a compulsor field");
        //        ModelState.AddModelError("Request Error", "Please check that the request contains a Customer Id");
        //        return BadRequest(ModelState);
        //    }
        //    CustomerSecurityPosition customerPosition = new CustomerSecurityPosition();
        //    try
        //    {
        //        var position = customerPosition.GetSecurityPositionByAccountNo(accountNumber, Config);
        //        return Ok(position);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogError(ex, "Unable to query Flexcube for the customer position");
        //        return StatusCode(500);
        //    }
        //}
    }
}
