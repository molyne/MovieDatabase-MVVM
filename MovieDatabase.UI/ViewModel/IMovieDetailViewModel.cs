using System.Threading.Tasks;

namespace MovieDatabase.UI.ViewModel
{
    public interface IMovieDetailViewModel
    {
        Task LoadAsync(int movieId);
        bool HasChanges { get; }
    }
}