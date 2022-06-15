using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace datingAppBackend.Models
{
	public class country
	{
        [Key]
        public int id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Country Name")]
        public string name { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Country Code")]
        public string code { get; set; }

        [DisplayName("Enabled")]
        public bool enabled { get; set; } = true;
        public DateTime createdOn { get; set; }
        public DateTime updatedOn { get; set; }
    }
}

