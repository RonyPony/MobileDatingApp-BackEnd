using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace datingAppBackend.Models
{
	public class matches
	{
        [Key]
        public int id { get; set; }

        [Required]
        [DisplayName("Usuario origen")]
        public int originUserId { get; set; }

        [Required]
        [DisplayName("Usuario Destino")]
        public int finalUserId { get; set; }

        [Required]
        [DisplayName("isAcepted")]
        public bool isAcepted { get; set; }

        [Required]
        [DisplayName("isSeen")]
        public bool isSeen { get; set; }
    }
}

