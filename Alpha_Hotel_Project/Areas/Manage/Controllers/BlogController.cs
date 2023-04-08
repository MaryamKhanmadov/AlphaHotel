using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Helpers;
using Alpha_Hotel_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Alpha_Hotel_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;
        public BlogController(AppDbContext appDbContext, IWebHostEnvironment env)
        {
            _appDbContext = appDbContext;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _appDbContext.Blogs.Include(x=>x.BlogCategory).Where(x=>x.IsDeleted==false).AsQueryable();
            PaginatedList<Blog> blogs = PaginatedList<Blog>.Create(query, 5, page);
            return View(blogs);
        }
        public IActionResult Create()
        {
            ViewBag.Categories = _appDbContext.BlogCategories.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Blog blog)
        {
            ViewBag.Categories = _appDbContext.BlogCategories.ToList();
            if (!ModelState.IsValid) { return View(blog); }
            if (blog.ImageFile is null)
            {
                ModelState.AddModelError("Image", "Required");
                return View(blog);
            }
            if (blog.ImageFile.ContentType != "image/jpeg" && blog.ImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("Image", "Only upload image PNG and JPEG format");
                return View(blog);
            }
            if (blog.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("Image", "Only you can upload 2mb files allowed");
                return View(blog);
            }
            blog.ImageUrl = blog.ImageFile.SaveFile(_env.WebRootPath, "uploads/blogs");
            _appDbContext.Add(blog);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(Guid id)
        {
            ViewBag.Categories = _appDbContext.BlogCategories.ToList();
            Blog blog = _appDbContext.Blogs.FirstOrDefault(x => x.Id == id);
            if (blog is null) { return NotFound(); }
            return View(blog);
        }
        [HttpPost]
        public IActionResult Update(Blog blog)
        {
            ViewBag.Categories = _appDbContext.BlogCategories.ToList();
            if (!ModelState.IsValid) { return View(blog); }
            Blog existblog = _appDbContext.Blogs.FirstOrDefault(x => x.Id == blog.Id);
            if (existblog is null) { return NotFound(); }

            if (blog.ImageFile != null)
            {
                if (blog.ImageFile.ContentType != "image/jpeg" && blog.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("Image", "Only PNG and JPEG files allowed");
                    return View(blog);
                }
                if (blog.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("Image", "Up to 2mb files allowed");
                    return View(blog);
                }
                string path = Path.Combine(_env.WebRootPath, "uploads/blogs", existblog.ImageUrl);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                existblog.ImageUrl = blog.ImageFile.SaveFile(_env.WebRootPath, "uploads/blogs");
            }
            existblog.Title = blog.Title;
            existblog.TwitterUrl = blog.TwitterUrl;
            existblog.Quote = blog.Quote;
            existblog.Descreption = blog.Descreption;
            existblog.SubHeadingDescreption = blog.SubHeadingDescreption;
            existblog.SubHeading = blog.SubHeading;
            existblog.FacebookUrl = blog.FacebookUrl;
            existblog.BlogCategoryId = blog.BlogCategoryId;
            existblog.LinkedinUrl = blog.LinkedinUrl;
            existblog.GoogleUrl = blog.GoogleUrl;
            existblog.PinterestUrl = blog.PinterestUrl;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(Guid id)
        {
            Blog blog = _appDbContext.Blogs.FirstOrDefault(x => x.Id == id);
            if (blog is null) return View("Error");
            blog.IsDeleted = true;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult SoftDeleteIndex(int page = 1)
        {
            var query = _appDbContext.Blogs.Include(x => x.BlogCategory).Where(x => x.IsDeleted == true).AsQueryable();
            PaginatedList<Blog> blogs = PaginatedList<Blog>.Create(query, 5, page);
            return View(blogs);
        }
        public IActionResult HardDelete(Guid id)
        {
            Blog blog = _appDbContext.Blogs.FirstOrDefault(x => x.Id == id);
            if (blog is null) return View("Error");
            string path = Path.Combine(_env.WebRootPath, "uploads/blogs", blog.ImageUrl);
            System.IO.File.Delete(path);
            List<BlogComment> blogComments = _appDbContext.BlogComments.Where(x => x.BlogId == blog.Id).ToList();
            foreach (var item in blogComments)
            {
                _appDbContext.BlogComments.Remove(item);
            }
            _appDbContext.Remove(blog);
            _appDbContext.SaveChanges();
            return RedirectToAction("SoftDeleteIndex");
        }
        public IActionResult Restore(Guid id)
        {
            Blog blog = _appDbContext.Blogs.FirstOrDefault(x => x.Id == id);
            if (blog == null) return View("Error");
            blog.IsDeleted = false;
            _appDbContext.SaveChanges();
            return RedirectToAction("SoftDeleteIndex");
        }
        public IActionResult BlogCommentIndex(Guid id , int page=1)
        {
            var query = _appDbContext.BlogComments.Include(x => x.Blog).Where(x => x.BlogId == id).AsQueryable();
            PaginatedList<BlogComment> comments = PaginatedList<BlogComment>.Create(query, 5, page);
            return View(comments);
        }
        public IActionResult DeleteComment(Guid id)
        {
            BlogComment comment = _appDbContext.BlogComments.Include(x => x.Blog).FirstOrDefault(y => y.BlogId == id);
            if (comment is null) return View("Error");
            comment.IsDeleted = true;
            //_appDbContext.Remove(comment);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
