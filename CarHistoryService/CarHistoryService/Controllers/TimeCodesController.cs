using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarHistoryService.Models;

namespace CarHistoryService.Controllers
{
    [Produces("application/json")]
    [Route("api/TimeCodes")]
    public class TimeCodesController : Controller
    {
        private readonly StateHistoryDbContext _context;

        public TimeCodesController(StateHistoryDbContext context)
        {
            _context = context;
        }

        // GET: api/TimeCodes
        [HttpGet]
        public IEnumerable<TimeCodeHistory> GetTimeCodes()
        {
            return _context.TimeCodes;
        }

        // GET: api/TimeCodes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTimeCodeHistory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var timeCodeHistory = await _context.TimeCodes.SingleOrDefaultAsync(m => m.Id == id);

            if (timeCodeHistory == null)
            {
                return NotFound();
            }

            return Ok(timeCodeHistory);
        }

        // PUT: api/TimeCodes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimeCodeHistory([FromRoute] int id, [FromBody] TimeCodeHistory timeCodeHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != timeCodeHistory.Id)
            {
                return BadRequest();
            }

            _context.Entry(timeCodeHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimeCodeHistoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TimeCodes
        [HttpPost]
        public async Task<IActionResult> PostTimeCodeHistory([FromBody] TimeCodeHistory timeCodeHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TimeCodes.Add(timeCodeHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTimeCodeHistory", new { id = timeCodeHistory.Id }, timeCodeHistory);
        }

        // DELETE: api/TimeCodes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimeCodeHistory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var timeCodeHistory = await _context.TimeCodes.SingleOrDefaultAsync(m => m.Id == id);
            if (timeCodeHistory == null)
            {
                return NotFound();
            }

            await _context.AccuStates.ForEachAsync(a =>
            {
                if (a.IdTimeCode == id)
                {
                    _context.AccuStates.Remove(a);
                }
            });
            _context.TimeCodes.Remove(timeCodeHistory);
            await _context.SaveChangesAsync();

            return Ok(timeCodeHistory);
        }

        private bool TimeCodeHistoryExists(int id)
        {
            return _context.TimeCodes.Any(e => e.Id == id);
        }
    }
}