using System;
namespace datingAppBackend.Models
{
    public sealed class PhotoToRegister
    {
        public IFormFile Image { get; set; }
        public int userId { get; set; }
    }
}

