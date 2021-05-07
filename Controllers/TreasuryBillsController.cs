using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DigitalProductAPI.Contexts;
using DigitalProductAPI.Entities;
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
    public class TreasuryBillsController : ControllerBase
    {
        private readonly SecuritiesContext _context;
        private readonly ILogger<TreasuryBillsController> Logger;

        public TreasuryBillsController(SecuritiesContext context, ILogger<TreasuryBillsController> logger)
        {
            _context = context;
            Logger = logger;
        }
        [HttpGet]
        public ActionResult Get(string StartDate, string EndDate, bool IsDefault)
        {
            Logger.LogInformation($"Treasury bill request ::: startDate = {StartDate}; EndDate = {EndDate}; IsDefault = {IsDefault}");
            if (!IsDefault)
            {
                if (string.IsNullOrWhiteSpace(StartDate) || string.IsNullOrWhiteSpace(EndDate))
                {
                    Logger.LogWarning($"start date and/or EndDate is null or empty");
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
                        var data = _context.TreasuryBills.Where(a => a.InputDate >= start && a.InputDate < end && a.IsAuthorized == false && a.Status == "UNAUTHORIZED").OrderByDescending(a => a.Id).ToList();
                        return Ok(data);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, $"Unable to get list of Treasury bills");
                        return StatusCode(500);
                    }
                }
                else
                {
                    Logger.LogWarning($"please check that your start date and end date are in the required format");

                    ModelState.AddModelError("Date Format", "please check that your start date and end date are in the required format");
                    return BadRequest(ModelState);
                }

            }
            try
            {
                var data = _context.TreasuryBills.Where(a => a.IsAuthorized == false && a.Status == "UNAUTHORIZED").OrderByDescending(a => a.Id).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Unable to get list of Treasury bills");

                return StatusCode(500);                
            }

        }
        [HttpPost]
        public ActionResult Post([FromBody] List<TreasuryBill> treasuryBills)
        {
            Logger.LogWarning($"Submit treasury bill request {JsonConvert.SerializeObject(treasuryBills)}");
            foreach (var item in treasuryBills)
            {
                try
                {
                    _context.TreasuryBills.Add(item);
                    _context.SaveChanges();                    
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Unable to save list of Treasury bills");
                    return StatusCode(500);
                }
            }
            return StatusCode(201);
        }
        [HttpPut]
        public ActionResult Put([FromBody] List<AuthorizeTreasuryBillRequest> authorizeRequest)
        {
            Logger.LogWarning($"Authorize treasury bills request ::: {JsonConvert.SerializeObject(authorizeRequest)}");
            foreach (var item in authorizeRequest)
            {
                try
                {
                    string query = $"update [DigitalProducts].[dbo].[TreasuryBills] set [AuthorizerId] = '{item.AuthorizerId}', [IsAuthorized] = 'true', [Status] = 'AUTHORIZED', [AuthorizeDate] = '{DateTime.Now}' where [Id] = {item.Id} ";
                    var data = _context.TreasuryBills.Where(a => a.Id == item.Id).FirstOrDefault();
                    if (data == null) return NotFound(item.Id);
                    _context.Database.ExecuteSqlRaw(query);
                    // _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Unable to authorize list of Treasury bills");
                    return StatusCode(500);
                }
            }            
            return NoContent();
        }
    }
}
