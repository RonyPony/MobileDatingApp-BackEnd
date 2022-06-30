using System;
using Microsoft.EntityFrameworkCore;
namespace datingAppBackend.Models
{
	public class datingContext:DbContext
	{
		public datingContext(DbContextOptions<datingContext>options):base(options)
		{

		}
		public DbSet<User> Users { get; set; } = null;
		public DbSet<country> Countries { get; set; } = null;
		public DbSet<matches> Matches { get; set; } = null;
		public DbSet<Photo> Photos { get; set; } = null;
		public DbSet<sexualOrientations> SexualOrientations { get; set; } = null;
	}
}

