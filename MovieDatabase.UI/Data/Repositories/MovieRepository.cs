using MovieDatabase.DataAccess;
using MovieDatabase.Model;
using System.Data.Entity;
using System.Threading.Tasks;

namespace MovieDatabase.UI.Data.Repositories
{
    public class MovieRepository : GenericRepository<Movie, MovieDatabaseDbContext>, IMovieRepository
    {
        public MovieRepository(MovieDatabaseDbContext context)
        : base(context)
        {

        }
        public override async Task<Movie> GetByIdAsync(int id)
        {
            return await Context.Movies
                .Include(d => d.Directors)
                .SingleAsync(movie => movie.Id == id);
        }

        public void RemoveDirector(Director selectedDirectorModel)
        {
            Context.Directors.Remove(selectedDirectorModel);
        }
    }
}
