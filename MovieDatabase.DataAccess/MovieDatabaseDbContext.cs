using MovieDatabase.Model;
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
        public DbSet<Actor> Directors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
