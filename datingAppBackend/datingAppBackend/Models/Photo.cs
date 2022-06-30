using System;
using System.ComponentModel.DataAnnotations;

namespace datingAppBackend.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

