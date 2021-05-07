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
    public class BondsController : ControllerBase
    {
        private readonly SecuritiesContext _context;
        private readonly ILogger<BondsController> Logger;

        public BondsController(SecuritiesContext context, ILogger<BondsController> logger)
        {
            _context = context;
            Logger = logger;
        }
        [HttpGet]
        public ActionResult Get(string StartDate, string EndDate, bool IsDefault)
        {
            Logger.LogInformation($"startDate = {StartDate}::: Enddate = {EndDate}::: IsDefault = {IsDefault}"); 
            if (!IsDefault)
            {
                if (string.IsNullOrWhiteSpace(StartDate) || string.IsNullOrWhiteSpace(EndDate))
                {
                    ModelState.AddModelError("request format", "start and end date cannot be null or empty if default is false");
                    Logger.LogWarning($"Start and/or end date is null");
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
                        var data = _context.Bonds.Where(a => a.InputDate >= start && a.InputDate < end && a.IsAuthorized == false && a.Status == "UNAUTHORIZED").OrderByDescending(a => a.Id).ToList();
                        return Ok(data);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, $"Unable to get unauthorized bond");
                        return StatusCode(500);
                    }
                }
                else
                {
                    ModelState.AddModelError("Date Format", "please check that your start date and end date are in the required format");
                    Logger.LogWarning($"startDate = {StartDate}::: Enddate = {EndDate}");
                    return BadRequest(ModelState);
                }

            }
            try
            {
                var data = _context.Bonds.Where(a => a.IsAuthorized == false && a.Status == "UNAUTHORIZED").OrderByDescending(a => a.Id).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                Logger.LogInformation(ex, $"Unable to get Unauthorized bonds");
                return StatusCode(500);
                //throw;
            }

        }

        [HttpPost]
        public ActionResult Post([FromBody] List<Bond> bonds)
        {
            Logger.LogInformation($"Save Bonds request ::: {JsonConvert.SerializeObject(bonds)}");
            foreach (var item in bonds)
            {
                try
                {
                    _context.Bonds.Add(item);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"unable to save the request");
                    return StatusCode(500);
                }
            }
            return StatusCode(201);
        }

        [HttpPut]
        public ActionResult Put([FromBody] List<AuthorizeBondRequest> authorizeRequest)
        {
            Logger.LogInformation($"Save Bonds request ::: {JsonConvert.SerializeObject(authorizeRequest)}");
            foreach (var item in authorizeRequest)
            {
                try
                {
                    string query = $"update [DigitalProducts].[dbo].[Bonds] set [AuthorizerId] = '{item.AuthorizerId}', [IsAuthorized] = 'true', [Status] = 'AUTHORIZED', [AuthorizeDate] = '{DateTime.Now}' where [Id] = {item.Id} ";
                    var data = _context.Bonds.Where(a => a.Id == item.Id).FirstOrDefault();
                    if (data == null) return NotFound(item.Id);
                    _context.Database.ExecuteSqlRaw(query);
                    // _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Unable to Authorize requests");
                    return StatusCode(500);
                }
            }
            return NoContent();
        }
    }
}
