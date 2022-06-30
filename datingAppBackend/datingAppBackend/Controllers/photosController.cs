
using datingAppBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace datingAppBackend.Controllers
{
    [Route("api/photos")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        //private readonly IPhotoService _photoService;
        private readonly datingContext _context;

        public PhotoController(datingContext ctx)
        {
            _context = ctx;
            //_photoService = photoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPhotos()
        {
            try
            {
                //IEnumerable<Photo> photos = await _photoService.GetPhotos();
                IEnumerable<Photo> photos = await _context.Photos.ToListAsync();
                return Ok(photos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            try
            {

                //Photo photo = await _photoService.GetPhotoById(id);
                Photo photo = await _context.Photos.FirstOrDefaultAsync(photo => photo.Id == id);

                return Ok(photo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetPhotosByUser([FromQuery] int userId)
        {
            try
            {
                //IEnumerable<Photo> photos = await _photoService.GetPhotosByUserId(userId);
                IEnumerable<Photo> photos = await _context.Photos.Where(r=>r.UserId == userId).ToListAsync();
                return Ok(photos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("new")]
        public async Task<IActionResult> PostPhoto([FromForm] PhotoToRegister photoToRegister)
        {
            try
            {
                string[] extensions = new string[] { ".jpg", ".png", ".svg", "jpeg" };
                var fileName = Path.GetFileName(photoToRegister.Image.FileName);
                var fileExtension = Path.GetExtension(fileName);

                if (!extensions.Contains(fileExtension))
                    throw new ArgumentException("{0} is an InvalidFileExtention", fileExtension);

                User foundUser = await _context.Users.FindAsync(photoToRegister.userId);

                if (foundUser is null)
                    NotFound("UserNotFound");

                var newPhoto = new Photo
                {
                    Name = fileName.Split('.')[0],
                    UserId = photoToRegister.userId,
                    CreatedAt = DateTime.Now
                };

                using (var target = new MemoryStream())
                {
                    photoToRegister.Image.CopyTo(target);
                    newPhoto.Image = target.ToArray();
                }

                //await _photoService.CreatePhoto(newPhoto);
                _context.Add(newPhoto);
                await _context.SaveChangesAsync();
                return StatusCode(201);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            try
            {
                //Photo photo = await _photoService.GetPhotoById(id);
                Photo photo = await _context.Photos.FindAsync(id);
                _context.Remove(photo);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
