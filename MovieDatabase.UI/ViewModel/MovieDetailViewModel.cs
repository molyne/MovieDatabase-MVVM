using MovieDatabase.Model;
using MovieDatabase.UI.Data.Repositories;
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
        private readonly IMovieRepository _movieRepository;
        private readonly IEventAggregator _eventAggregator;
        private MovieWrapper _movie;
        private bool _hasChanges;

        public MovieDetailViewModel(IMovieRepository movieRepository, IEventAggregator eventAggregator)
        {
            _movieRepository = movieRepository;
            _eventAggregator = eventAggregator;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
        }

        public async Task LoadAsync(int? movieId)
        {
            var movie = movieId.HasValue
                ? await _movieRepository.GetByIdAsync(movieId.Value)
                : CreateNewMovie();
            Movie = new MovieWrapper(movie);
            Movie.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _movieRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Movie.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Movie.Id == 0)
            {
                //Little trick to trigger the validation
                Movie.Title = "";
            }
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
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public bool HasChanges
        {
            get => _hasChanges;
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        private bool OnSaveCanExecute()
        {
            return Movie != null && !Movie.HasErrors && HasChanges;
        }

        private async void OnSaveExecute()
        {
            await _movieRepository.SaveAsync();
            HasChanges = _movieRepository.HasChanges();
            _eventAggregator.GetEvent<AfterMovieSavedEvent>().Publish(
                new AfterMovieSavedEventArgs
                {
                    Id = Movie.Id,
                    DisplayMember = $"{Movie.Title}"
                }
            );
        }

        private Movie CreateNewMovie()
        {
            var movie = new Movie();

            _movieRepository.Add(movie);

            return movie;
        }

        private async void OnDeleteExecute()
        {
            _movieRepository.Remove(Movie.Model);
            await _movieRepository.SaveAsync();
            _eventAggregator.GetEvent<AfterMovieDeletedEvent>().Publish(Movie.Id);
        }
    }
}
