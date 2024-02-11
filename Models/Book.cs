using System.ComponentModel.DataAnnotations.Schema;

namespace books_api.Models
{
    public class Book
    {
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Editor { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
    }
}
