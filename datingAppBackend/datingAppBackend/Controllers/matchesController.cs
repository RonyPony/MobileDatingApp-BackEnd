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
    public class matchesController : ControllerBase
    {
        private readonly datingContext _context;

        public matchesController(datingContext context)
        {
            _context = context;
        }

        // GET: api/matches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<matches>>> GetMatches()
        {
          if (_context.Matches == null)
          {
              return NotFound();
          }
            return await _context.Matches.ToListAsync();
        }

        // GET: api/matches/getPossibleMatch
        [HttpGet]
        [Route("getPossibleMatch/{userId}")]
        public async Task<ActionResult<IEnumerable<matches>>> GetPossibleMatch(int userId)
        {
            if (_context.Matches == null)
            {
                return NotFound();
            }
            if (userId==null)
            {
                return BadRequest();
            }

            bool appropiateUserFound=false;

            try
            {
                List<User> allUsers = await _context.Users.ToListAsync();
                if (allUsers.Count() < 10)
                {
                    return StatusCode(StatusCodes.Status404NotFound,"Not enough registered users, add more than 10");
                }
                User originUser = await _context.Users.FindAsync(userId);
                User destiUser = new User();
                while (!appropiateUserFound)
                {
                    destiUser = getArandomUser(originUser);
                    bool tmpVal = validateFoundUser(destiUser, originUser);
                    appropiateUserFound = tmpVal;
                }
                return Ok(destiUser);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex);
            }
        }

        private bool validateFoundUser(User destiUser,User originUser)
        {
            bool finalValidation = false;
            if (destiUser.id==originUser.id)
            {
                return false;
            }
            return true;
        }

        private User getArandomUser(User originUser)
        {
            User usr = new User();
            if (originUser.sexualPreferenceId!=0)
            {
                usr = _context.Users.OrderBy(r => Guid.NewGuid())
                    .Where(r => r.sexualPreferenceId == originUser.sexualPreferenceId)
                    .Where(r => r.id!=originUser.id)
                    .Take(5).FirstOrDefault();
                return usr;
            }
            usr = _context.Users.OrderBy(r => Guid.NewGuid()).Take(5).FirstOrDefault();
            return usr;

        }

        // GET: api/matches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<matches>> Getmatches(int id)
        {
          if (_context.Matches == null)
          {
              return NotFound();
          }
            var matches = await _context.Matches.FindAsync(id);

            if (matches == null)
            {
                return NotFound();
            }

            return matches;
        }

        // PUT: api/matches/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putmatches(int id, matches matches)
        {
            if (id != matches.id)
            {
                return BadRequest();
            }

            _context.Entry(matches).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!matchesExists(id))
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

        // POST: api/matches
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<matches>> Postmatches(matchDto matchesDto)
        {
            matches matches = new matches() { finalUserId = matchesDto.finalUserId, isAcepted = matchesDto.isAcepted, originUserId=matchesDto.originUserId};
          if (_context.Matches == null)
          {
              return Problem("Entity set 'datingContext.Matches'  is null.");
          }
            _context.Matches.Add(matches);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getmatches", new { id = matches.id }, matches);
        }

        // DELETE: api/matches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletematches(int id)
        {
            if (_context.Matches == null)
            {
                return NotFound();
            }
            var matches = await _context.Matches.FindAsync(id);
            if (matches == null)
            {
                return NotFound();
            }

            _context.Matches.Remove(matches);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool matchesExists(int id)
        {
            return (_context.Matches?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
