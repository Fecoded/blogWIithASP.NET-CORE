using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBlog.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using Microsoft.AspNetCore.Http;

namespace WebBlog.Controllers
{
  // [ApiController]
  // [Route("api/[controller]")]
  public class BlogController : Controller
  {
    private readonly BlogDbContext context;
    private readonly IWebHostEnvironment webHostEnvironment;
    public BlogController(BlogDbContext context, IWebHostEnvironment hostEnvironment)
    {
      this.context = context;
      this.webHostEnvironment = hostEnvironment;
    }

    // DISPLAY ALL POST
    [HttpGet]
    public async Task<IActionResult> Index()
    {
      return View(await context.BlogModels.ToListAsync());
    }

    // GET ALL POST
    [HttpGet]
    public async Task<IActionResult> Details()
    {
      return View(await context.BlogModels.ToListAsync());
    }

    // GET POST BY ID
    [HttpGet("Detail/{id}")]
    public async Task<ActionResult<BlogModel>> Detail(int id)
    {
      return View(await context.BlogModels.FirstOrDefaultAsync(x => x.Id == id));
    }

    // POST: blog/create;
    public IActionResult Create()
    {
      return View();
    }

    public IActionResult Contact()
    {
      return View();
    }

    public IActionResult About()
    {
      return View();
    }

    // CREATE A POST
    // POST: Blog/Create
    [HttpPost]
    public async Task<IActionResult> Create([Bind("Id,Title,Description,ImageFile")] BlogModel blogModel)
    {
      if (ModelState.IsValid)
      {
        // Save Image to wwwRoot Image Folder
        string wwwRootPath = webHostEnvironment.WebRootPath;
        string fileName = Path.GetFileNameWithoutExtension(blogModel.ImageFile.FileName);
        string extension = Path.GetExtension(blogModel.ImageFile.FileName);
        blogModel.ImagePath = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
        string path = Path.Combine(wwwRootPath + "/img/", fileName);
        using (var fileStream = new FileStream(path, FileMode.Create))
        {
          await blogModel.ImageFile.CopyToAsync(fileStream);
        }
        // Insert Record
        context.Add(blogModel);
        await context.SaveChangesAsync();
        return RedirectToAction("Index");
      }
      return View(blogModel);
    }

    // UPDATE A POST
    public IActionResult Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }
      BlogModel blogModel = context.BlogModels.Find(id);
      if (blogModel == null)
      {
        return NotFound();
      }

      return View(blogModel);
    }

    [HttpPost, ActionName("Edit")]
    public async Task<ActionResult> Edit(BlogModel blogModel)
    {
      if (ModelState.IsValid)
      {

        // if (blogModel.ImageFile != null)
        // {
        //   string wwwRootPath = webHostEnvironment.WebRootPath;
        //   string fileName = Path.GetFileNameWithoutExtension(blogModel.ImageFile.FileName);
        //   string extension = Path.GetExtension(blogModel.ImageFile.FileName);
        //   fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
        //   string path = Path.Combine(wwwRootPath + "/img/", fileName);
        //   using (var fileStream = new FileStream(path, FileMode.Create))
        //   {
        //     await blogModel.ImageFile.CopyToAsync(fileStream);
        //   }
        // }
        context.Entry(blogModel).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return RedirectToAction("Details");
      }
      return View(blogModel);
    }

    // DELETE A POST
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }
      var imageModel = await context.BlogModels.FirstOrDefaultAsync(m => m.Id == id);
      if (imageModel == null)
      {
        return NotFound();
      }
      return View(imageModel);
    }


    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
      var blogModel = await context.BlogModels.FindAsync(id);
      var ImagePath = Path.Combine(webHostEnvironment.WebRootPath, "img", blogModel.ImagePath);
      if (System.IO.File.Exists(ImagePath))
        System.IO.File.Delete(ImagePath);
      context.BlogModels.Remove(blogModel);
      await context.SaveChangesAsync();
      return RedirectToAction("Details");
    }

  }
}