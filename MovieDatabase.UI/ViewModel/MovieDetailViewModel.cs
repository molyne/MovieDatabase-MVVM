using MovieDatabase.Model;
using MovieDatabase.UI.Data.Lookups;
using MovieDatabase.UI.Data.Repositories;
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
    public class MovieDetailViewModel : DetailViewModelBase, IMovieDetailViewModel
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IGenreLookupDataService _genreLookupDataService;
        private MovieWrapper _movie;
        private DirectorWrapper _selectedDirector;

        public MovieDetailViewModel(IMovieRepository movieRepository, IEventAggregator eventAggregator, IMessageDialogService messageDialogService, IGenreLookupDataService genreLookupDataService)
        : base(eventAggregator)
        {
            _movieRepository = movieRepository;
            _messageDialogService = messageDialogService;
            _genreLookupDataService = genreLookupDataService;

            AddDirectorCommand = new DelegateCommand(OnAddDirectorExecute);
            RemoveDirectorCommand = new DelegateCommand(OnRemoveDirectorExecute, OnRemoveDirectorCanExecute);

            Genres = new ObservableCollection<LookupItem>();
            Directors = new ObservableCollection<DirectorWrapper>();
        }

        public ObservableCollection<LookupItem> Genres { get; set; }
        public ObservableCollection<DirectorWrapper> Directors { get; }

        public override async Task LoadAsync(int? movieId)
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

        public ICommand AddDirectorCommand { get; }
        public ICommand RemoveDirectorCommand { get; }

        public DirectorWrapper SelectedDirector
        {
            get => _selectedDirector;
            set
            {
                _selectedDirector = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveDirectorCommand).RaiseCanExecuteChanged();
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return Movie != null
                   && !Movie.HasErrors
                   && Directors.All(d => !d.HasErrors)
                   && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await _movieRepository.SaveAsync();
            HasChanges = _movieRepository.HasChanges();
            RaiseDetailSavedEvent(Movie.Id, Movie.Title);
        }

        protected override async void OnDeleteExecute()
        {
            var result =
                _messageDialogService.ShowOkCancelDialog($"Do you really want to delete the movie {Movie.Title}?",
                    "Question");

            if (result == MessageDialogResult.OK)
            {
                _movieRepository.Remove(Movie.Model);
                await _movieRepository.SaveAsync();
                RaiseDetailDeletedEvent(Movie.Id);
            }
        }

        private Movie CreateNewMovie()
        {
            var movie = new Movie();

            _movieRepository.Add(movie);

            return movie;
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
            SelectedDirector.PropertyChanged -= DirectorWrapper_PropertyChanged;
            _movieRepository.RemoveDirector(SelectedDirector.Model);
            Directors.Remove(SelectedDirector);
            SelectedDirector = null;
            HasChanges = _movieRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemoveDirectorCanExecute()
        {
            return SelectedDirector != null;
        }

        private void OnAddDirectorExecute()
        {
            var newDirector = new DirectorWrapper(new Director());
            newDirector.PropertyChanged += DirectorWrapper_PropertyChanged;
            Directors.Add(newDirector);
            Movie.Model.Directors.Add(newDirector.Model);
            newDirector.Name = ""; //Trigger validation
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
