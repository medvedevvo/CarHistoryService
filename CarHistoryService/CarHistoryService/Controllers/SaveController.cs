using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarHistoryService.Models;
using Microsoft.Extensions.Logging;

namespace CarHistoryService.Controllers
{
    [Produces("application/json")]
    [Route("api/Save")]
    public class SaveController : Controller
    {
        private readonly StateHistoryDbContext _context;
        private ILogger<SaveController> logger;
        
        public SaveController(StateHistoryDbContext context, ILogger<SaveController> logger)
        {
            logger.LogDebug($"Created SaveController");
            _context = context;
            this.logger = logger;
        }

        // POST: api/save
        [HttpPost]
        public async Task<IActionResult> PostCarService([FromBody] AccuListWithTime accu)
        {
            logger.LogDebug($"Start saving car current state");
            if (!ModelState.IsValid)
            {
                logger.LogDebug($"Error: ModelState is not correct");
                return BadRequest(ModelState);
            }

            logger.LogDebug($"Start saving time code");
            TimeCodeHistory timeCodeHistory = new TimeCodeHistory(accu.idCar, accu.time);
            _context.TimeCodes.Add(timeCodeHistory);
            await _context.SaveChangesAsync();
            int idTimeCode = timeCodeHistory.Id;
            logger.LogDebug($"Time code saved. Start saving accu history");

            foreach (Accu a in accu.accu)
            {
                AccuHistory accuHistory = new AccuHistory(idTimeCode, a.id, a.name, a.voltage, a.current, a.charge);
                _context.AccuStates.Add(accuHistory);
            }
            await _context.SaveChangesAsync();
            logger.LogDebug($"Accu history saved");

            return Ok();
        }
    }
}