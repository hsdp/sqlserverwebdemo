using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SqlServerWebDemo.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}