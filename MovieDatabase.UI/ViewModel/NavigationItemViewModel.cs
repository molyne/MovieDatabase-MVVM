namespace MovieDatabase.UI.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        private string _displayMember;
        public int Id { get; }

        public NavigationItemViewModel(int id, string displayMember)
        {
            Id = id;
            _displayMember = displayMember;
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
    }
}
