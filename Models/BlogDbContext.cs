using Microsoft.EntityFrameworkCore;

namespace WebBlog.Models
{
  public class BlogDbContext : DbContext
  {
    public DbSet<BlogModel> BlogModels { get; set; }
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }
  }
}