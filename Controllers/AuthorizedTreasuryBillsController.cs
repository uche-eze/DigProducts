using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DigitalProductAPI.Contexts;
using DigitalProductAPI.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DigitalProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthorizedTreasuryBillsController : ControllerBase
    {
        private readonly SecuritiesContext _context;
        private readonly ILogger<AuthorizedTreasuryBillsController> Logger;

        public AuthorizedTreasuryBillsController(SecuritiesContext context, ILogger<AuthorizedTreasuryBillsController> logger)
        {
            _context = context;
            Logger = logger;
        }
        [HttpPut]
        public ActionResult Put([FromBody] AuthorizeTreasuryBillRequest cancellationRequest)
        {
            Logger.LogInformation($"Cancelation request ::: {JsonConvert.SerializeObject(cancellationRequest)}");
            string query = $"update [DigitalProducts].[dbo].[TreasuryBills] set [AuthorizerId] = '{cancellationRequest.AuthorizerId}', [IsAuthorized] = 'false', [Status] = 'CANCELLED' , [AuthorizeDate] = {DateTime.Now} where [Id] = {cancellationRequest.Id} ";
            try
            {                
                var data = _context.TreasuryBills.Where(a => a.Id == cancellationRequest.Id).FirstOrDefault();
                if (data == null) return NotFound(cancellationRequest.Id);
                _context.Database.ExecuteSqlRaw(query);
                // _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"error occured while running the update::: {query}");
                return StatusCode(500);
            }
            return NoContent();
        }

        [HttpGet]
        public ActionResult Get(string StartDate, string EndDate, bool IsDefault)
        {
            Logger.LogInformation($"startDate = {StartDate} ::: enddate = {EndDate} ::: isDefault = {IsDefault}");
            if (!IsDefault)
            {
                if (string.IsNullOrWhiteSpace(StartDate) || string.IsNullOrWhiteSpace(EndDate))
                {
                    ModelState.AddModelError("request format", "start and end date cannot be null or empty if default is false");
                    return BadRequest(ModelState);
                }
                if (DateTime.TryParse(EndDate, new CultureInfo("en-gb"),
                        DateTimeStyles.None, out _) &&
                        DateTime.TryParse(StartDate, new CultureInfo("en-gb"),
                        DateTimeStyles.None, out _))
                {
                    DateTime start = Convert.ToDateTime(StartDate);
                    DateTime end = Convert.ToDateTime(EndDate);
                    try
                    {
                        string query = $"SELECT * FROM [DigitalProducts].[dbo].[TreasuryBills] where Status = 'AUTHORIZED' and InputDate in (select max(InputDate) from[DigitalProducts].[dbo].[TreasuryBills] where status = 'AUTHORIZED' group by SecurityID)";
                        var data = _context.TreasuryBills.FromSqlRaw(query).OrderByDescending(a => a.Id).ToList();
                        //var data = _context.TreasuryBills.Where(a => a.InputDate >= start && a.InputDate < end && a.IsAuthorized == true && a.InputDate.).OrderByDescending(a => a.Id).ToList();
                        return Ok(data);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, $"unable to get Authorized Treasury bills list");
                        return StatusCode(500);
                    }
                }
                else
                {
                    ModelState.AddModelError("Date Format", "please check that your start date and end date are in the required format");
                    Logger.LogWarning($"Invalid date format :::startDate = {StartDate} ::: enddate = {EndDate}");
                    return BadRequest(ModelState);
                }

            }
            try
            {
                string query = $"SELECT * FROM [DigitalProducts].[dbo].[TreasuryBills] where Status = 'AUTHORIZED' and InputDate in (select max(InputDate) from[DigitalProducts].[dbo].[TreasuryBills] where status = 'AUTHORIZED' group by SecurityID)";
                var data = _context.TreasuryBills.FromSqlRaw(query).OrderByDescending(a => a.Id).Take(50).ToList();
                //var data = _context.TreasuryBills.Where(a => a.Id <= 30 && a.IsAuthorized == true).OrderByDescending(a => a.Id).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"unable to get Authorized Treasury bills list");
                return StatusCode(500);
                //throw;
            }
        }
    }
}
