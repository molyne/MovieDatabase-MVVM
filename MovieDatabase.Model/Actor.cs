using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Model
{
    public class Actor
    {
        public Actor()
        {
            Movies = new Collection<Movie>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [RegularExpression("[A-Za-z ]*", ErrorMessage = "Invalid Name ")]
        public string Name { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}
