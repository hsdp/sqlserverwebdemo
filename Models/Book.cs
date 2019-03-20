using System.ComponentModel.DataAnnotations;

namespace SqlServerWebDemo.Models
{
    public class Book
    {
        public int BookId { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public int AuthorId { get; set; }
        
        public Author Author { get; set; }
    }
}