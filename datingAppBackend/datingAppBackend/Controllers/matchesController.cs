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
                List<matches> allMatchesFromThisUser = await _context.Matches.Where(r=>r.originUserId == userId).ToListAsync();
                int usersCount = allUsers.Count;
                int matchesFromThisUserCount = allMatchesFromThisUser.Count;
                if (matchesFromThisUserCount == usersCount||matchesFromThisUserCount>(usersCount-2))
                {
                    return BadRequest("Ups, al parecer ya has agotado todas las opciones actuales, en unas pocas horas te analizaremos mas 🔥🤖.");
                }
                if (usersCount < 10)
                {
                    return StatusCode(StatusCodes.Status404NotFound,"Not enough registered users, add more than 10");
                }
                User originUser = await _context.Users.FindAsync(userId);
                User destiUser = new User();
                
                while (!appropiateUserFound)
                {
                    destiUser = getArandomUser(originUser);
                    bool hasmatch = validateExistingMatch(originUser, destiUser);
                    bool tmpVal = validateFoundUser(destiUser, originUser);

                    if (!hasmatch && tmpVal)
                    {
                        appropiateUserFound = true;
                    }
                    else
                    {
                        appropiateUserFound = false;
                    }
                }
                return Ok(destiUser);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex);
            }
        }

        private bool validateExistingMatch(User? originUser, User destiUser)
        {
            matches matchex = _context.Matches
                .Where(r => r.originUserId == originUser.id && r.finalUserId == destiUser.id)
                .FirstOrDefault();

            return matchex != null;
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
                    .Where(r => r.sexualPreferenceId == originUser.sexualPreferenceId && r.id != originUser.id && r.isEnabled).FirstOrDefault();
                return usr;
            }
            usr = _context.Users.OrderBy(r => Guid.NewGuid()).Where(r=>r.id!=originUser.id && r.isEnabled).Take(5).FirstOrDefault();
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



        // GET: api/matches/userMatches/5
        [HttpGet("userMatches/{userId}")]
        public async Task<ActionResult<List<matches>>> getUsersMatches(int userId)
        {
            if (_context.Matches == null)
            {
                return NotFound();
            }
            List<matches> matches = await _context.Matches
                .Where(e=>e.finalUserId == userId && e.isAcepted)             
                .ToListAsync();
            List<matches> matchesBackwards = await _context.Matches
                .Where(e => e.originUserId == userId && e.isAcepted)
                .ToListAsync();
            //var backwardsMatches = await _context.Matches
            //    .Where(e => e.originUserId == userId && e.isAcepted)
            //    .ToListAsync();

            if (matches == null)
            {
                return NoContent();
            }

            return matches.Union(matchesBackwards).OrderBy(x => x.id).ToList();
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
            User originUser = await _context.Users.FindAsync(matchesDto.originUserId);
            User destinUser = await _context.Users.FindAsync(matchesDto.finalUserId);
            //bool hasMatch = validateExistingMatch(originUser,destinUser);
            bool hasMatchBackwards = validateExistingMatch(destinUser, originUser);
            if (hasMatchBackwards)
            {
                matches match =_context.Matches
                    .Where(r => r.originUserId == destinUser.id)
                    .Where(r => r.finalUserId == originUser.id)
                    .FirstOrDefault();
                match.isAcepted = true;
                _context.Matches.Update(match);
                _context.SaveChangesAsync();
                return Ok("These users matched");
            }
            else
            {
                matches matches = new matches() { finalUserId = matchesDto.finalUserId, isAcepted = matchesDto.isAcepted, originUserId = matchesDto.originUserId };
                if (_context.Matches == null)
                {
                    return Problem("Entity set 'datingContext.Matches'  is null.");
                }
                _context.Matches.Add(matches);
                await _context.SaveChangesAsync();

                return CreatedAtAction("Getmatches", new { id = matches.id }, matches);
            }
            
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
