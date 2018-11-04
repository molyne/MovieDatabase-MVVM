using MovieDatabase.Model;
using MovieDatabase.UI.Data.Lookups;
using MovieDatabase.UI.Data.Repositories;
using MovieDatabase.UI.Event;
using MovieDatabase.UI.View.Services;
using MovieDatabase.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovieDatabase.UI.ViewModel
{
    public class MovieDetailViewModel : ViewModelBase, IMovieDetailViewModel
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IGenreLookupDataService _genreLookupDataService;
        private MovieWrapper _movie;
        private bool _hasChanges;
        private DirectorWrapper _selectedDirector;

        public MovieDetailViewModel(IMovieRepository movieRepository, IEventAggregator eventAggregator, IMessageDialogService messageDialogService, IGenreLookupDataService genreLookupDataService)
        {
            _movieRepository = movieRepository;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _genreLookupDataService = genreLookupDataService;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
            AddDirectorCommand = new DelegateCommand(OnAddDirectorExecute);
            RemoveDirectorCommand = new DelegateCommand(OnRemoveDirectorExecute, OnRemoveDirectorCanExecute);

            Genres = new ObservableCollection<LookupItem>();
            Directors = new ObservableCollection<DirectorWrapper>();
        }

        public ObservableCollection<LookupItem> Genres { get; set; }
        public ObservableCollection<DirectorWrapper> Directors { get; }

        public async Task LoadAsync(int? movieId)
        {
            var movie = movieId.HasValue
                ? await _movieRepository.GetByIdAsync(movieId.Value)
                : CreateNewMovie();

            InitializeMovie(movie);

            InitializeDirectors(movie.Directors);

            await LoadGenresLookupAsync();
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
        public ICommand AddDirectorCommand { get; }
        public ICommand RemoveDirectorCommand { get; }

        public DirectorWrapper SelectedDirector
        {
            get { return _selectedDirector; }
            set
            {
                _selectedDirector = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveDirectorCommand).RaiseCanExecuteChanged();
            }
        }

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
            return Movie != null
                   && !Movie.HasErrors
                   && Directors.All(d => !d.HasErrors)
                   && HasChanges;
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
            var result =
                _messageDialogService.ShowOkCancelDialog($"Do you really want to delete the movie {Movie.Title}?",
                    "Question");

            if (result == MessageDialogResult.OK)
            {
                _movieRepository.Remove(Movie.Model);
                await _movieRepository.SaveAsync(); _eventAggregator.GetEvent<AfterMovieDeletedEvent>().Publish(Movie.Id);
            }
        }

        private void InitializeMovie(Movie movie)
        {
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

        private async Task LoadGenresLookupAsync()
        {
            Genres.Clear();
            Genres.Add(new NullLookupItem { DisplayMember = " - " });
            var lookup = await _genreLookupDataService.GetGenreLookupAsync();
            foreach (var lookupItem in lookup)
            {
                Genres.Add(lookupItem);
            }
        }

        private void OnRemoveDirectorExecute()
        {
            //TODO Implement this       
        }

        private bool OnRemoveDirectorCanExecute()
        {
            return SelectedDirector != null;
        }

        private void OnAddDirectorExecute()
        {
            //TODO Implement this
        }

        private void DirectorWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _movieRepository.HasChanges();
            }

            if (e.PropertyName == nameof(DirectorWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }
        private void InitializeDirectors(ICollection<Director> directors)
        {
            foreach (var wrapper in Directors)
            {
                wrapper.PropertyChanged -= DirectorWrapper_PropertyChanged;
            }
            Directors.Clear();
            foreach (var director in directors)
            {
                var wrapper = new DirectorWrapper(director);
                Directors.Add(wrapper);
                wrapper.PropertyChanged += DirectorWrapper_PropertyChanged;
            }
        }
    }
}
