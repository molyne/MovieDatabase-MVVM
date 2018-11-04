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
            Directors = new Collection<Director>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public TimeSpan Duration { get; set; }

        public ICollection<Director> Directors { get; set; }

        public int? MovieGenreId { get; set; }
        public Genre MovieGenre { get; set; }

    }
}
