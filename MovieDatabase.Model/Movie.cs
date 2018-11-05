using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Model
{
    public class Movie
    {
        public Movie()
        {
            Actors = new Collection<Actor>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public TimeSpan Duration { get; set; }

        public ICollection<Actor> Actors { get; set; }

        public int? MovieGenreId { get; set; }
        public Genre MovieGenre { get; set; }

    }
}
