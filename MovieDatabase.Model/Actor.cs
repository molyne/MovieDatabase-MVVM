using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Model
{
    public class Actor
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [RegularExpression("[A-Za-z ]*", ErrorMessage = "Invalid Name ")]
        public string Name { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
