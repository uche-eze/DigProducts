using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalProductAPI.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DigitalProductAPI.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuditLogHistoryController : ControllerBase
    {
        private readonly SecuritiesContext _context;
        private readonly ILogger<AuditLogHistoryController> Logger;

        public AuditLogHistoryController(SecuritiesContext context, ILogger<AuditLogHistoryController> logger)
        {
            _context = context;
            Logger = logger;
        }
        [Route("api/Bonds/[controller]")]
        [HttpGet]
        public ActionResult GetBonds()
        {
            Logger.LogInformation("Request to get all bonds transactions for Audit log purpose");
            try
            {
                var data = _context.Bonds.OrderByDescending(a => a.Id).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to get bonds transaction history");
                return StatusCode(500);
            }
        }
        [Route("api/TreasuryBills/[controller]")]
        [HttpGet]
        public ActionResult GetTBills()
        {
            Logger.LogInformation("Request to get all Treasury Bills transactions for Audit log purpose");
            try
            {
                var data = _context.TreasuryBills.OrderByDescending(a => a.Id).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to get bonds transaction history");
                return StatusCode(500);
            }
        }
    }
}
