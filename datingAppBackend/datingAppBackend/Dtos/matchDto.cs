using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace datingAppBackend.Dtos
{
	public class matchDto
	{
        [Required]
        [DisplayName("Usuario origen")]
        public int originUserId { get; set; }

        [Required]
        [DisplayName("Usuario Destino")]
        public int finalUserId { get; set; }

        [Required]
        [DisplayName("isAcepted")]
        public bool isAcepted { get; set; }
    }
}

