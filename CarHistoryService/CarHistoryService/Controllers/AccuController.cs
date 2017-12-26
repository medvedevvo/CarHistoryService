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
    [Route("api/Accus")]
    public class AccuController : Controller
    {
        private readonly StateHistoryDbContext _context;

        public AccuController(StateHistoryDbContext context)
        {
            _context = context;
        }

        // GET: api/Accu
        [HttpGet]
        public IEnumerable<AccuHistory> GetAccuStates()
        {
            return _context.AccuStates;
        }

        // GET: api/Accu/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccuHistory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accuHistory = await _context.AccuStates.SingleOrDefaultAsync(m => m.Id == id);

            if (accuHistory == null)
            {
                return NotFound();
            }

            return Ok(accuHistory);
        }

        // PUT: api/Accu/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccuHistory([FromRoute] int id, [FromBody] AccuHistory accuHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != accuHistory.Id)
            {
                return BadRequest();
            }

            _context.Entry(accuHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccuHistoryExists(id))
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

        // POST: api/Accu
        [HttpPost]
        public async Task<IActionResult> PostAccuHistory([FromBody] AccuHistory accuHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AccuStates.Add(accuHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccuHistory", new { id = accuHistory.Id }, accuHistory);
        }

        // DELETE: api/Accu/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccuHistory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accuHistory = await _context.AccuStates.SingleOrDefaultAsync(m => m.Id == id);
            if (accuHistory == null)
            {
                return NotFound();
            }

            _context.AccuStates.Remove(accuHistory);
            await _context.SaveChangesAsync();

            return Ok(accuHistory);
        }

        private bool AccuHistoryExists(int id)
        {
            return _context.AccuStates.Any(e => e.Id == id);
        }
    }
}