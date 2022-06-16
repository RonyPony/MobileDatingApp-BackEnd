using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using datingAppBackend.Models;
using datingAppBackend.Dtos;

namespace datingAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class countriesController : ControllerBase
    {
        private readonly datingContext _context;

        public countriesController(datingContext context)
        {
            _context = context;
        }

        // GET: api/countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<country>>> GetCountries()
        {
          if (_context.Countries == null)
          {
              return NotFound();
          }
            return await _context.Countries.ToListAsync();
        }

        // GET: api/countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<country>> Getcountry(int id)
        {
          if (_context.Countries == null)
          {
              return NotFound();
          }
            var country = await _context.Countries.FindAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }

        // PUT: api/countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putcountry(int id, country country)
        {
            if (id != country.id)
            {
                return BadRequest();
            }
            country.updatedOn = DateTime.Now;
            _context.Entry(country).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!countryExists(id))
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

        // POST: api/countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<country>> Postcountry(registerCountryDto countryDto)
        {
            country country = new country(){
                code=countryDto.code,
                createdOn = DateTime.Now,
                enabled = countryDto.enabled,
                name=countryDto.name,
                updatedOn=DateTime.Now
            };
          if (_context.Countries == null)
          {
              return Problem("Entity set 'datingContext.Countries'  is null.");
          }
            country.createdOn = DateTime.Now;
            country.updatedOn = DateTime.Now;
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getcountry", new { id = country.id }, country);
        }

        // DELETE: api/countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletecountry(int id)
        {
            if (_context.Countries == null)
            {
                return NotFound();
            }
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            country.enabled = false;
            _context.Entry(country).State = EntityState.Modified;
            //_context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool countryExists(int id)
        {
            return (_context.Countries?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
