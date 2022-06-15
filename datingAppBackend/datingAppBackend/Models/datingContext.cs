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
	}
}

