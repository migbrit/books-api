using System.ComponentModel.DataAnnotations.Schema;

namespace books_api.Models
{
    public class Book
    {
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public string? Author { get; set; }
        public string? Editor { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
