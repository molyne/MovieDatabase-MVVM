using MovieDatabase.Model;
using System.Threading.Tasks;

namespace MovieDatabase.UI.Data.Repositories
{
    public interface IMovieRepository
    {
        Task<Movie> GetByIdAsync(int id);
        Task SaveAsync();
        bool HasChanges();
    }
}