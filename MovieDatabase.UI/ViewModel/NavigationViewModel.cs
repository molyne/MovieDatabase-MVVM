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

        public NavigationViewModel(IMovieLookupDataService movieLookupService, IEventAggregator eventAggregator)
        {
            _movieLookupService = movieLookupService;
            _eventAggregator = eventAggregator;
            Movies = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterMovieSavedEvent>().Subscribe(AfterMovieSaved);
            _eventAggregator.GetEvent<AfterMovieDeletedEvent>().Subscribe(AfterMovieDeleted);

        }

        public ObservableCollection<NavigationItemViewModel> Movies { get; }

        public async Task LoadAsync()
        {
            var lookup = await _movieLookupService.GetMovieLookupAsync();
            Movies.Clear();
            foreach (var item in lookup)
            {
                Movies.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, _eventAggregator));
            }
        }

        private void AfterMovieDeleted(int movieId)
        {
            var movie = Movies.SingleOrDefault(m => m.Id == movieId);
            if (movie != null)
            {
                Movies.Remove(movie);
            }
        }

        private void AfterMovieSaved(AfterMovieSavedEventArgs obj)
        {
            var lookupItem = Movies.SingleOrDefault(m => m.Id == obj.Id);
            if (lookupItem == null)
            {
                Movies.Add(new NavigationItemViewModel(obj.Id, obj.DisplayMember, _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = obj.DisplayMember;
            }
        }
    }
}
