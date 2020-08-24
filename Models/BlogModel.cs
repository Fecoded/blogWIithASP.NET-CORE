using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace WebBlog.Models
{
  public class BlogModel
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }

    [NotMapped]
    public IFormFile ImageFile { get; set; }
  }
}