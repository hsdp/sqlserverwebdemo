using Microsoft.EntityFrameworkCore;

namespace SqlServerWebDemo.Models
{
    public class DemoContext : DbContext
    {
        public DemoContext(DbContextOptions options) : base(options) {}

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
    }
}