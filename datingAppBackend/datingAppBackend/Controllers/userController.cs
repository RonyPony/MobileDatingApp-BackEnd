using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using datingAppBackend.Models;
using datingAppBackend.Dtos;
using datingAppBackend.Enums;

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
            return await _context.Users.Where(e=>e.isEnabled==true).ToListAsync();
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
        public async Task<ActionResult<User>> PostUser(registerUserDto userRegister)
        {
          if (_context.Users == null)
          {
              return Problem("Entity set 'datingContext.Users'  is null.");
          }

            var usersFound = await _context.Users
                .Where(x=>x.email==userRegister!.Email)
                .ToListAsync();
            bool emailExists = usersFound.Count<=0;
            if (emailExists)
            {
                User user = new User();
                user.name = userRegister.UserName;
                user.email = userRegister.Email;
                user.Password = userRegister.Password;
                user.lastName = userRegister.lastName;
                user.isEnabled = true;
                user.bio = "N/A";
                user.sexualOrientationId = 0;
                user.sexualPreferenceId = 0;
                user.registerDate = DateTime.UtcNow;
                user.lastLogin = DateTime.UtcNow;
                user.deletedAccount = false;
                user.instagramUser = "N/A";
                user.instagramUserEnabled = false;
                user.loginStatus = LoginStatus.New;
                user.maximunAgeToMatch = 80;
                user.minimunAgeToMatch = 18;
                user.modoFantasma = false;
                user.whatsappNumberEnabled = false;
                user.whatsappNumber = "N/A";
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUser", new { id = user.id }, user);
            }
            else
            {
                return BadRequest("This email is already registered, try to login");
            }
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
        public async Task<ActionResult<User>> loginUser(loginDto userLogin)
        {
            userLogin.UserEmail = userLogin.UserEmail.ToLower();
            if (_context.Users == null)
            {
                return Problem("Entity set 'datingContext.Users'  is null.");
            }

            Console.WriteLine("Performing login to user"+userLogin.UserEmail);
            var users = (from x in _context.Users
                         where x.email == userLogin.UserEmail
                         where x.Password == userLogin.Password
                         
                         select x).FirstOrDefault();
            int userId = 0;

            if (!users.isEnabled)
            {
                return StatusCode(StatusCodes.Status423Locked,"Current User ("+users.email+") is locked by the admin, we will review your account just to make sure, keep an eye on your email inbox, or contact us");
            }

            if (users != null)
            {
                userId = users.id;
            }
            else
            {
                return Unauthorized("Wrong information provided");
            }

            User user = await _context.Users.FindAsync(userId);

            user.lastLogin = DateTime.UtcNow;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }



        // POST: api/enableUser/1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("enableUser/{id}")]
        public async Task<ActionResult<User>> enableUser(int id)
        {

            if (_context.Users == null)
            {
                return Problem("Entity set 'datingContext.Users'  is null.");
            }
            

            if (id!= null)
            {
                User user = await _context.Users.FindAsync(id);

                user.isEnabled = true;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return Ok(user);
            }
            else
            {
                return BadRequest("Provide ID");
            }

            
        }

        // POST: api/desableUser/1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("disableUser/{id}")]
        public async Task<ActionResult<User>> disableUser(int id)
        {

            if (_context.Users == null)
            {
                return Problem("Entity set 'datingContext.Users'  is null.");
            }


            if (id != null)
            {
                User user = await _context.Users.FindAsync(id);

                user.isEnabled = false;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return Ok(user);
            }
            else
            {
                return BadRequest("Provide ID");
            }


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
