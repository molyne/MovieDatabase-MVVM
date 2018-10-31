using MovieDatabase.Model;
using System;
using System.Runtime.CompilerServices;

namespace MovieDatabase.UI.Wrapper
{
    public class ModelWrapper<T> : NotifyDataErrorInfoBase
    {
        public T Model;

        public ModelWrapper(T model)
        {
            Model = model;
        }
        protected virtual TValue GetValue<TValue>([CallerMemberName]string propertyName = null)
        {
            return (TValue)typeof(T).GetProperty(propertyName).GetValue(Model);
        }
        protected virtual void SetValue<TValue>(TValue value,
            [CallerMemberName]string propertyName = null)
        {
            typeof(T).GetProperty(propertyName).SetValue(Model, value);
            OnPropertyChanged(propertyName);
        }

    }
    public class MovieWrapper : ModelWrapper<Movie>
    {


        public MovieWrapper(Movie model) : base(model)
        {
        }

        public int Id => Model.Id;

        public string Title
        {
            get => GetValue<string>(nameof(Title));
            set
            {
                SetValue(value);
                ValidateProperty(nameof(Title));
            }
        }



        private void ValidateProperty(string propertyName)
        {
            ClearErrors(propertyName);

            switch (propertyName)
            {
                case nameof(Title):
                    if (string.Equals(Title, "Robot", StringComparison.OrdinalIgnoreCase))
                    {
                        AddError(propertyName, "Robots are not valid movies");
                    }
                    break;
            }
        }

        public TimeSpan Duration
        {
            get => GetValue<TimeSpan>();
            set => SetValue(value);
        }

        public Director Director
        {
            get => GetValue<Director>();
            set => SetValue(value);
        }


    }
}
