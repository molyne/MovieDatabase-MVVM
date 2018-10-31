using MovieDatabase.UI.Data;
using MovieDatabase.UI.Event;
using MovieDatabase.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovieDatabase.UI.ViewModel
{
    public class MovieDetailViewModel : ViewModelBase, IMovieDetailViewModel
    {
        private readonly IMovieDataService _dataService;
        private readonly IEventAggregator _eventAggregator;
        private MovieWrapper _movie;
        public ICommand SaveCommand { get; }

        public MovieDetailViewModel(IMovieDataService dataService, IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<OpenMovieDetailViewEvent>().Subscribe(OnOpenMovieDetailView);
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        public async Task LoadAsync(int movieId)
        {
            var movie = await _dataService.GetByIdAsync(movieId);
            Movie = new MovieWrapper(movie);
        }

        public MovieWrapper Movie
        {
            get => _movie;
            private set
            {
                _movie = value;
                OnPropertyChanged();
            }
        }

        private bool OnSaveCanExecute()
        {
            //TODO check if friend is valid
            return true;
        }

        private async void OnSaveExecute()
        {
            await _dataService.SaveAsync(Movie.Model);
            _eventAggregator.GetEvent<AfterMovieSavedEvent>().Publish(
                new AfterMovieSavedEventArgs
                {
                    Id = Movie.Id,
                    DisplayMember = $"{Movie.Title}"
                }
                );
        }

        private async void OnOpenMovieDetailView(int movieId)
        {
            await LoadAsync(movieId);
        }
    }
}
