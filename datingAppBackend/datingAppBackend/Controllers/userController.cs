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
    public class userController : ControllerBase
    {
        private readonly datingContext _context;

        public userController(datingContext context)
        {
            _context = context;
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            return await _context.Users.ToListAsync();
        }

        // GET: api/user/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/user/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/user
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(RegisterModel userRegister)
        {
          if (_context.Users == null)
          {
              return Problem("Entity set 'datingContext.Users'  is null.");
          }
            if (userRegister.Password!=userRegister.ConfirmPassword)
            {
                return Problem("Password does not match");
            }
            User user = new User();
            user.name = userRegister.UserName;
            user.email = userRegister.Email;
            user.Password = userRegister.Password;
            user.lastName = userRegister.lastName;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.id }, user);
        }

        [HttpPost]
        [Route("findByEmail/{email}")]
        public async Task<ActionResult<User>>findUserByEmail(String email)
        {
            if (email==String.Empty)
            {
                return BadRequest("Please provide an email address");
            }

            try
            {
                var user = (from x in _context.Users
                             where x.email == email
                             select x).FirstOrDefault();
                if (user==null)
                {
                    return NotFound("User not found");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/login
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<User>> loginUser(Login userLogin)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'datingContext.Users'  is null.");
            }
            var users = (from x in _context.Users
                         where x.email == userLogin.UserEmail
                         select x).FirstOrDefault();

            int userId = 0;

            if (users != null)
            {
                userId = users.id;
            }

            User user = await _context.Users.FindAsync(userId);

            user.lastLogin = DateTime.Now;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        // DELETE: api/user/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
