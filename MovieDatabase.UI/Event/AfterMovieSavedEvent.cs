using Prism.Events;

namespace MovieDatabase.UI.Event
{
    public class AfterMovieSavedEvent : PubSubEvent<AfterMovieSavedEventArgs>
    {
    }

    public class AfterMovieSavedEventArgs
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
    }
}
