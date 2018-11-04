using MovieDatabase.DataAccess;
using MovieDatabase.Model;
using System.Data.Entity;
using System.Threading.Tasks;

namespace MovieDatabase.UI.Data.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieDatabaseDbContext _context;

        public MovieRepository(MovieDatabaseDbContext context)
        {
            _context = context;
        }
        public async Task<Movie> GetByIdAsync(int id)
        {
            return await _context.Movies.SingleAsync(movie => movie.Id == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public void Add(Movie movie)
        {
            _context.Movies.Add(movie);
        }

        public void Remove(Movie model)
        {
            _context.Movies.Remove(model);
        }
    }
}
