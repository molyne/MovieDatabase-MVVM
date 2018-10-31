using System.Threading.Tasks;

namespace MovieDatabase.UI.ViewModel
{
    public interface INavigationViewModel
    {
        Task LoadAsync();
    }
}