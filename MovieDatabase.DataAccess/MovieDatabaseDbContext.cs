using MovieDatabase.Model;
using System;
using System.Data.Entity;

namespace MovieDatabase.DataAccess
{
    public class MovieDatabaseDbContext : DbContext
    {
        public MovieDatabaseDbContext() : base("MovieDatabaseDb")
        {

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
        }
    }
}
