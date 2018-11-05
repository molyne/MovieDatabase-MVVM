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
        private ActorWrapper _selectedActor;

        public MovieDetailViewModel(IMovieRepository movieRepository, IEventAggregator eventAggregator, IMessageDialogService messageDialogService, IGenreLookupDataService genreLookupDataService)
        : base(eventAggregator)
        {
            _movieRepository = movieRepository;
            _messageDialogService = messageDialogService;
            _genreLookupDataService = genreLookupDataService;

            AddActorCommand = new DelegateCommand(OnAddActorExecute);
            RemoveActorCommand = new DelegateCommand(OnRemoveActorExecute, OnRemoveActorCanExecute);

            Genres = new ObservableCollection<LookupItem>();
            Actors = new ObservableCollection<ActorWrapper>();
        }

        public ObservableCollection<LookupItem> Genres { get; set; }
        public ObservableCollection<ActorWrapper> Actors { get; }

        public override async Task LoadAsync(int? movieId)
        {
            var movie = movieId.HasValue
                ? await _movieRepository.GetByIdAsync(movieId.Value)
                : CreateNewMovie();

            InitializeMovie(movie);

            InitializeActors(movie.Actors);

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

        public ICommand AddActorCommand { get; }
        public ICommand RemoveActorCommand { get; }

        public ActorWrapper SelectedActor
        {
            get => _selectedActor;
            set
            {
                _selectedActor = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveActorCommand).RaiseCanExecuteChanged();
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return Movie != null
                   && !Movie.HasErrors
                   && Actors.All(d => !d.HasErrors)
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

        private void OnRemoveActorExecute()
        {
            SelectedActor.PropertyChanged -= ActorWrapper_PropertyChanged;
            _movieRepository.RemoveDirector(SelectedActor.Model);
            Actors.Remove(SelectedActor);
            SelectedActor = null;
            HasChanges = _movieRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemoveActorCanExecute()
        {
            return SelectedActor != null;
        }

        private void OnAddActorExecute()
        {
            var newActor = new ActorWrapper(new Actor());
            newActor.PropertyChanged += ActorWrapper_PropertyChanged;
            Actors.Add(newActor);
            Movie.Model.Actors.Add(newActor.Model);
            newActor.Name = ""; //Trigger validation
        }

        private void ActorWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _movieRepository.HasChanges();
            }

            if (e.PropertyName == nameof(ActorWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private void InitializeActors(ICollection<Actor> actors)
        {
            foreach (var wrapper in Actors)
            {
                wrapper.PropertyChanged -= ActorWrapper_PropertyChanged;
            }
            Actors.Clear();
            foreach (var actor in actors)
            {
                var wrapper = new ActorWrapper(actor);
                Actors.Add(wrapper);
                wrapper.PropertyChanged += ActorWrapper_PropertyChanged;
            }
        }
    }
}
