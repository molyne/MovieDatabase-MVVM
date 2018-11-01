using MovieDatabase.UI.Data.Lookups;
using MovieDatabase.UI.Event;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private readonly IMovieLookupDataService _movieLookupService;
        private readonly IEventAggregator _eventAggregator;
        public ObservableCollection<NavigationItemViewModel> Movies { get; }

        public NavigationViewModel(IMovieLookupDataService movieLookupService, IEventAggregator eventAggregator)
        {
            _movieLookupService = movieLookupService;
            _eventAggregator = eventAggregator;
            Movies = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterMovieSavedEvent>().Subscribe(AfterMovieSaved);

        }

        private void AfterMovieSaved(AfterMovieSavedEventArgs obj)
        {
            var lookupItem = Movies.Single(m => m.Id == obj.Id);
            lookupItem.DisplayMember = obj.DisplayMember;
        }

        public async Task LoadAsync()
        {
            var lookup = await _movieLookupService.GetMovieLookupAsync();
            Movies.Clear();
            foreach (var item in lookup)
            {
                Movies.Add(new NavigationItemViewModel(item.Id, item.DisplayMember));
            }
        }

        private NavigationItemViewModel _selectedMovie;
        public NavigationItemViewModel SelectedMovie
        {
            get => _selectedMovie;
            set
            {
                _selectedMovie = value;
                OnPropertyChanged();
                if (_selectedMovie != null)
                {
                    _eventAggregator.GetEvent<OpenMovieDetailViewEvent>()
                        .Publish(_selectedMovie.Id);
                }
            }
        }
    }
}
