using MovieDatabase.Model;
using System.Threading.Tasks;

namespace MovieDatabase.UI.Data
{
    public interface IMovieDataService
    {
        Task<Movie> GetByIdAsync(int id);
        Task SaveAsync(Movie movie);

    }
}