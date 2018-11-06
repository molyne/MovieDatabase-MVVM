using MovieDatabase.Model;
using System.Collections.ObjectModel;
using System.Linq;

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

            context.Genres.AddOrUpdate(g => g.Name,
                new Genre { Name = "Horror" },
                new Genre { Name = "Romance" },
                new Genre { Name = "Drama" },
                new Genre { Name = "Thriller" },
                new Genre { Name = "Action" },
                new Genre { Name = "Adventure" },
                new Genre { Name = "Sci-fi" },
                new Genre { Name = "Fantasy" },
                new Genre { Name = "Animated" },
                new Genre { Name = "Comedy" }
            );



            context.SaveChanges();

            var genres = context.Genres.ToList();

            context.Movies.AddOrUpdate(movie => movie.Title,
                new Movie
                {
                    Title = "Harry Potter and the Sorcerer's Stone",
                    ReleaseDate = new DateTime(2001, 11, 23),
                    Description = "An orphaned boy enrolls in a school of wizardry, where he learns the truth about himself, his family and the terrible evil that haunts the magical world.",
                    MovieGenreId = genres[7].Id
                },

                new Movie
                {
                    Title = "Star Wars The Last Jedi",
                    ReleaseDate = new DateTime(2017, 12, 13),
                    Description = "Rey develops her newly discovered abilities with the guidance of Luke Skywalker, who is unsettled by the strength of her powers. Meanwhile, the Resistance prepares for battle with the First Order.",
                    MovieGenreId = genres[6].Id
                },

                new Movie
                {
                    Title = "The Lion King",
                    ReleaseDate = new DateTime(1994, 11, 18),
                    Description = "A Lion cub crown prince is tricked by a treacherous uncle into thinking he caused his father's death and flees into exile in despair, only to learn in adulthood his identity and his responsibilities.",
                    MovieGenreId = genres[8].Id
                },

                new Movie
                {
                    Title = "Mr. Bean's Holiday",
                    ReleaseDate = new DateTime(2007, 03, 30),
                    Description = "Mr. Bean wins a trip to Cannes where he unwittingly separates a young boy from his father and must help the two come back together. On the way he discovers France, bicycling, and true love, among other things.",
                    MovieGenreId = genres[9].Id
                });

            context.SaveChanges();

            var movies = context.Movies.ToList();


            context.Actors.AddOrUpdate(d => d.Name,
                new Actor { Name = "Daniel Radcliffe", Movies = new Collection<Movie> { movies[0] } },
                new Actor { Name = "Daisy Ridley", Movies = new Collection<Movie> { movies[1] } },
                new Actor { Name = "John Boyega", Movies = new Collection<Movie> { movies[1] } },
                new Actor { Name = "Rowan Atkinson", Movies = new Collection<Movie> { movies[2], movies[3] } }
                );
        }
    }
}

