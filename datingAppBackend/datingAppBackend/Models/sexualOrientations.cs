using System;
using System.ComponentModel.DataAnnotations;

namespace datingAppBackend.Models
{
	public class sexualOrientations
	{
		[Key]
		public int id { get; set; }
		public String name { get; set; }
		public int imageId { get; set; }
		public bool enabled { get; set; }
	}
}

