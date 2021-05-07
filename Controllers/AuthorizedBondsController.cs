﻿using System;
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
    public class AuthorizedBondsController : ControllerBase
    {
        private readonly SecuritiesContext _context;
        private readonly ILogger<AuthorizedBondsController> Logger;

        public AuthorizedBondsController(SecuritiesContext context, ILogger<AuthorizedBondsController> logger)
        {
            _context = context;
            Logger = logger;
        }

        [HttpGet]
        public ActionResult Get(string StartDate, string EndDate, bool IsDefault)
        {
            Logger.LogInformation($"Authorized Bond Request::: startDate: {StartDate}; endDate: {EndDate}; IsDefault:{IsDefault}");
            //IQueryable<TreasuryBill> query = context.TreasuryBills; 
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
                        string query = $"SELECT * FROM [DigitalProducts].[dbo].[Bonds] where Status = 'AUTHORIZED' and InputDate in (select max(InputDate) from [DigitalProducts].[dbo].[Bonds] where status = 'AUTHORIZED' group by SecurityID)";
                        var data = _context.Bonds.FromSqlRaw(query).OrderByDescending(a => a.Id).ToList();
                        return Ok(data);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "An error occured attemping to get the list of authorized and available bonds for internet banking");
                        return StatusCode(500);
                    }
                }
                else
                {
                    ModelState.AddModelError("Date Format", "please check that your start date and end date are in the required format");
                    Logger.LogWarning($"The date format used is not consistent to a particular date format::: startDate: {StartDate} ::: endDate: {EndDate}");
                    return BadRequest(ModelState);
                }

            }
            try
            {

                string query = $"SELECT * FROM [DigitalProducts].[dbo].[Bonds] where Status = 'AUTHORIZED' and InputDate in (select max(InputDate) from [DigitalProducts].[dbo].[Bonds] where status = 'AUTHORIZED' group by SecurityID)";
                var data = _context.Bonds.FromSqlRaw(query).OrderByDescending(a => a.Id).Take(50).ToList();
                //var data = _context.Bonds.Where(a => a.Id <= 30 && a.IsAuthorized == true).OrderByDescending(a => a.Id).ToList();
                Logger.LogInformation("Successfully got the default list of Authorized and available bonds for internet banking");
                return Ok(data);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occured attemping to get the list of authorized and available bonds for internet banking");
                return StatusCode(500);
                throw;
            }

        }

        [HttpPut]
        public ActionResult Put([FromBody] AuthorizeBondRequest cancellationRequest)
        {
            Logger.LogInformation($"Cancellation Request :::{JsonConvert.SerializeObject(cancellationRequest)}");
            try
            {
                string query = $"update [DigitalProducts].[dbo].[Bonds] set [AuthorizerId] = '{cancellationRequest.AuthorizerId}', [IsAuthorized] = 'false', [Status] = 'CANCELLED', [AuthorizeDate] = {DateTime.Now} where [Id] = {cancellationRequest.Id} ";
                var data = _context.Bonds.Where(a => a.Id == cancellationRequest.Id).FirstOrDefault();
                if (data == null) return NotFound(cancellationRequest.Id);
                _context.Database.ExecuteSqlRaw(query);
                // _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.LogInformation(ex, $"unable to cancel the request");
                return StatusCode(500);
            }
            return NoContent();
        }
    }
}
