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
		public DbSet<poucevanjeEntity> poucevanje { get; set; }
		public DbSet<gradivaEntity> gradiva { get; set; }
        public DbSet<nalogeEntity> naloge { get; set; }
        public DbSet<opravljene_nalogeEntity> opravljene_Naloge { get; set; }

    }
}

	 