using System;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Model
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        public Director Director { get; set; }
    }
}
