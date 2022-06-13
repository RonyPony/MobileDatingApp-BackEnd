using System;
using System.ComponentModel.DataAnnotations;

namespace datingAppBackend.Models
{
	public class User
	{
		[Key]
		public int id { get; set; }
		public string name { get; set; }
		public string lastName { get; set; }
	}
}

