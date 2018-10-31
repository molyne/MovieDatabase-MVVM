using MovieDatabase.DataAccess;
using MovieDatabase.Model;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace MovieDatabase.UI.Data
{
    public class MovieDataService : IMovieDataService
    {
        private readonly Func<MovieDatabaseDbContext> _contextCreator;

        public MovieDataService(Func<MovieDatabaseDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }
        public async Task<Movie> GetByIdAsync(int id)
        {
            using (var context = _contextCreator())
            {
                return await context.Movies.AsNoTracking().SingleAsync(movie => movie.Id == id);
            }
        }

        public async Task SaveAsync(Movie movie)
        {
            using (var context = _contextCreator())
            {
                context.Movies.Attach(movie);
                context.Entry(movie).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}
