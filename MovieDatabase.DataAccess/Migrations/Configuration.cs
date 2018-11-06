using MovieDatabase.Model;
using System;
using System.Linq;

namespace MovieDatabase.DataAccess.Migrations
{
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


            context.Movies.AddOrUpdate(movie => movie.Title,
                new Movie { Title = "Harry Potter och de vises sten", ReleaseDate = new DateTime(2001, 11, 23) },
                new Movie { Title = "Star Wars", ReleaseDate = new DateTime(2001, 11, 23) },
                new Movie { Title = "Lejonkungen", ReleaseDate = new DateTime(2001, 11, 23) },
                new Movie { Title = "Mr Bean" });

            context.Genres.AddOrUpdate(g => g.Name,
                new Genre { Name = "Horror" },
                new Genre { Name = "Romance" },
                new Genre { Name = "Drama" },
                new Genre { Name = "Thriller" },
                new Genre { Name = "Action" },
                new Genre { Name = "Adventure" },
                new Genre { Name = "Sci-fi" },
                new Genre { Name = "Fantasy" },
                new Genre { Name = "Animated" }
                );

            context.SaveChanges();

            context.Actors.AddOrUpdate(d => d.Name,
                new Actor { Name = "Daniel Radcliffe", MovieId = context.Movies.First().Id });
        }
    }
}
