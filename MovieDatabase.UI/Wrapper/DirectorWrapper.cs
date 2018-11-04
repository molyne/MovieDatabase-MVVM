﻿using MovieDatabase.Model;

namespace MovieDatabase.UI.Wrapper
{
    public class DirectorWrapper : ModelWrapper<Director>
    {
        public DirectorWrapper(Director model) : base(model)
        {

        }

        public string FirstName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
    }
}
