using System.Threading.Tasks;

namespace MovieDatabase.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public IMovieDetailViewModel MovieDetailViewModel { get; }
        public INavigationViewModel NavigationViewModel { get; }

        public MainViewModel(INavigationViewModel navigationViewModel, IMovieDetailViewModel movieDetailViewModel)
        {
            NavigationViewModel = navigationViewModel;
            MovieDetailViewModel = movieDetailViewModel;
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }
    }
}
