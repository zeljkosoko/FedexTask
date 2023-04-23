using Microsoft.EntityFrameworkCore;

namespace FedexTask.Models
{
    public class ArticleDbContext: DbContext
    {
        public ArticleDbContext(DbContextOptions<ArticleDbContext> options):base(options)
        { }

        public DbSet<Article> Article { get; set; }
    }
}
