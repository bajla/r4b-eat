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

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // Configure relationships using Fluent API
        //    modelBuilder.Entity<poucevanjeEntity>()
        //        .HasMany(e => e.)

        //    modelBuilder.Entity<Poucevanje>()
        //        .HasRequired(p => p.Predmet)
        //        .WithMany()
        //        .HasForeignKey(p => p.IdPredmeta);

        //    // Configure other relationships here...

        //    base.OnModelCreating(modelBuilder);
        //}
    }
}

	 