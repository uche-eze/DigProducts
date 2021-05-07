using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalProductAPI.Contexts;
using DigitalProductAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DigitalProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CurrencyController : ControllerBase
    {
        public ILogger<CurrencyController> Logger { get; }
        public SecuritiesContext _context { get; }
        public CurrencyController(ILogger<CurrencyController> logger, SecuritiesContext context)
        {
            Logger = logger;
            _context = context;
        }
        [HttpGet]
        public ActionResult Get()
        {
            Logger.LogInformation("Request to get get currency Codes");
            try
            {
                var data = _context.Currencies.OrderBy(a => a.Id).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to get Currency codes");
                return StatusCode(500);
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] List<Currency> currenciesRequest)
        {
            Logger.LogInformation($"Save Currency request ::: {JsonConvert.SerializeObject(currenciesRequest)}");
            foreach (var item in currenciesRequest)
            {
                try
                {
                    _context.Currencies.Add(item);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"unable to save currency request");
                    return StatusCode(500);
                }
            }
            return StatusCode(201);
        }
    }
}
