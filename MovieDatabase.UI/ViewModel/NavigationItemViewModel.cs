using MovieDatabase.UI.Event;
using Prism.Commands;
using Prism.Events;
using System.Windows.Input;

namespace MovieDatabase.UI.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        private string _displayMember;
        private readonly IEventAggregator _eventAggregator;
        public int Id { get; }

        public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator)
        {
            Id = id;
            _displayMember = displayMember;
            _eventAggregator = eventAggregator;
            OpenMovieDetailViewCommand = new DelegateCommand(OnOpenMovieDetailView);
        }

        public string DisplayMember
        {
            get => _displayMember;
            set
            {
                _displayMember = value;
                OnPropertyChanged();
            }
        }
        public ICommand OpenMovieDetailViewCommand { get; }

        private void OnOpenMovieDetailView()
        {
            _eventAggregator.GetEvent<OpenMovieDetailViewEvent>()
                .Publish(Id);
        }
    }
}
