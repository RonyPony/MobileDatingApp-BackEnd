using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using datingAppBackend.Models;

namespace datingAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SexualOrientationsController : ControllerBase
    {
        private readonly datingContext _context;

        public SexualOrientationsController(datingContext context)
        {
            _context = context;
        }

        // GET: api/SexualOrientations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<sexualOrientations>>> GetSexualOrientations()
        {
          if (_context.SexualOrientations == null)
          {
              return NotFound();
          }
            return await _context.SexualOrientations.ToListAsync();
        }

        // GET: api/SexualOrientations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<sexualOrientations>> GetsexualOrientations(int id)
        {
          if (_context.SexualOrientations == null)
          {
              return NotFound();
          }
            var sexualOrientations = await _context.SexualOrientations.FindAsync(id);

            if (sexualOrientations == null)
            {
                return NotFound();
            }

            return sexualOrientations;
        }

        // PUT: api/SexualOrientations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutsexualOrientations(int id, sexualOrientations sexualOrientations)
        {
            if (id != sexualOrientations.id)
            {
                return BadRequest();
            }

            _context.Entry(sexualOrientations).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!sexualOrientationsExists(id))
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

        // POST: api/SexualOrientations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<sexualOrientations>> PostsexualOrientations(sexualOrientations sexualOrientations)
        {
          if (_context.SexualOrientations == null)
          {
              return Problem("Entity set 'datingContext.SexualOrientations'  is null.");
          }
            _context.SexualOrientations.Add(sexualOrientations);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetsexualOrientations", new { id = sexualOrientations.id }, sexualOrientations);
        }

        // DELETE: api/SexualOrientations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletesexualOrientations(int id)
        {
            if (_context.SexualOrientations == null)
            {
                return NotFound();
            }
            var sexualOrientations = await _context.SexualOrientations.FindAsync(id);
            if (sexualOrientations == null)
            {
                return NotFound();
            }

            _context.SexualOrientations.Remove(sexualOrientations);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool sexualOrientationsExists(int id)
        {
            return (_context.SexualOrientations?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
