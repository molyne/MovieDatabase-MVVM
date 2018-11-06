using MovieDatabase.Model;
using System;
using System.Collections.Generic;

namespace MovieDatabase.UI.Wrapper
{
    public class MovieWrapper : ModelWrapper<Movie>
    {
        public MovieWrapper(Movie model) : base(model)
        {
        }

        public int Id => Model.Id;

        public string Title
        {
            get => GetValue<string>(nameof(Title));
            set => SetValue(value);
        }

        public string Description
        {
            get => GetValue<string>(nameof(Description));
            set => SetValue(value);
        }

        public Actor Actor
        {
            get => GetValue<Actor>();
            set => SetValue(value);
        }

        public int? MovieGenreId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        public DateTime ReleaseDate
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }


        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Title):
                    if (string.Equals(Title, "Robot", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return "Robots are not valid movies";
                    }
                    break;
            }
        }

    }
}
