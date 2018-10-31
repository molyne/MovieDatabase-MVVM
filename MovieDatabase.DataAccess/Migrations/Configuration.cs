using MovieDatabase.Model;

namespace MovieDatabase.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<MovieDatabase.DataAccess.MovieDatabaseDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MovieDatabase.DataAccess.MovieDatabaseDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.


            context.Movies.AddOrUpdate(movie => movie.Title, new Movie
            {
                Title = "Harry Potter och de vises sten",
                Duration = new TimeSpan(2, 32, 0)
            },
                new Movie { Title = "Star Wars" },
                new Movie { Title = "Lejonkungen" },
                new Movie { Title = "Mr Bean" });

        }
    }
}
