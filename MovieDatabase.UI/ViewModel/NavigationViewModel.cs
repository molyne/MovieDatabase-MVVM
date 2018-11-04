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
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);

        }

        public ObservableCollection<NavigationItemViewModel> Movies { get; }

        public async Task LoadAsync()
        {
            var lookup = await _movieLookupService.GetMovieLookupAsync();
            Movies.Clear();
            foreach (var item in lookup)
            {
                Movies.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, nameof(MovieDetailViewModel), _eventAggregator));
            }
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(MovieDetailViewModel):

                    var movie = Movies.SingleOrDefault(m => m.Id == args.Id);
                    if (movie != null)
                    {
                        Movies.Remove(movie);
                    }
                    break;
            }
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs obj)
        {
            switch (obj.ViewModelName)
            {
                case nameof(MovieDetailViewModel):

                    var lookupItem = Movies.SingleOrDefault(m => m.Id == obj.Id);
                    if (lookupItem == null)
                    {
                        Movies.Add(new NavigationItemViewModel(obj.Id, obj.DisplayMember, nameof(MovieDetailViewModel),
                            _eventAggregator));
                    }
                    else
                    {
                        lookupItem.DisplayMember = obj.DisplayMember;
                    }
                    break;
            }
        }
    }
}
