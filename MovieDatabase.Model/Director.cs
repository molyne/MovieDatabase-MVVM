using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Model
{
    public class Director
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
