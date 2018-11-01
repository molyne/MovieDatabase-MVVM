using MovieDatabase.UI.Event;
using MovieDatabase.UI.View.Services;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace MovieDatabase.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        private readonly Func<IMovieDetailViewModel> _movieDetailViewModelCreator;
        private IMovieDetailViewModel _movieDetailViewModel;


        public MainViewModel(INavigationViewModel navigationViewModel, Func<IMovieDetailViewModel> movieDetailViewModelCreator, IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            _movieDetailViewModelCreator = movieDetailViewModelCreator;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _eventAggregator.GetEvent<OpenMovieDetailViewEvent>().Subscribe(OnOpenMovieDetailView);
            NavigationViewModel = navigationViewModel;
        }

        public INavigationViewModel NavigationViewModel { get; }


        public IMovieDetailViewModel MovieDetailViewModel
        {
            get => _movieDetailViewModel;
            private set { _movieDetailViewModel = value; OnPropertyChanged(); }
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }
        private async void OnOpenMovieDetailView(int movieId)
        {
            if (MovieDetailViewModel != null && MovieDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog("You've made changes. Navigate away?", "Question");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            MovieDetailViewModel = _movieDetailViewModelCreator();
            await MovieDetailViewModel.LoadAsync(movieId);
        }

    }
}
