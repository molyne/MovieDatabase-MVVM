using MovieDatabase.Model;

namespace MovieDatabase.UI.Wrapper
{
    public class ActorWrapper : ModelWrapper<Actor>
    {
        public ActorWrapper(Actor model) : base(model)
        {

        }

        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
    }
}
