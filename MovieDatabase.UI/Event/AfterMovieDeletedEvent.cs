using Prism.Events;

namespace MovieDatabase.UI.Event
{
    public class AfterMovieDeletedEvent : PubSubEvent<int>
    {
    }
}
