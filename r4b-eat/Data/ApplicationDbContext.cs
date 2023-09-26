using System;
using r4b_eat.Models;
using Microsoft.EntityFrameworkCore;

namespace r4b_eat.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{

		}

		public DbSet<uporabnikiEntity> uporabniki { get; set; }
		public DbSet<predmetiEntity> predmeti { get; set; }
	}
}

	 