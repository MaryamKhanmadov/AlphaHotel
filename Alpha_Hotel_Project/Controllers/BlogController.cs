using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Helpers;
using Alpha_Hotel_Project.Migrations;
using Alpha_Hotel_Project.Models;
using Alpha_Hotel_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Alpha_Hotel_Project.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public BlogController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index(int page=1)
        {
            List<Partner> partners = _appDbContext.Partners.ToList();
            var query = _appDbContext.Blogs.Include(x => x.BlogCategory).Include(x => x.BlogComments).Include(x => x.BlogCategory).Where(x => x.IsDeleted == false).AsQueryable();
            BlogViewModel blogVM = new BlogViewModel
            {
                Blogs = PaginatedList<Blog>.Create(query, 2, page),
                RecentBlogComment = _appDbContext.BlogComments.OrderByDescending(x => x.MessageTime).Where(x => x.IsDeleted == false).Take(5).ToList(),
                BlogCategories = _appDbContext.BlogCategories.Include(x => x.Blogs).Where(x => x.IsDeleted == false).ToList(),
                Partners = partners,
                RecentBlogs = _appDbContext.Blogs.OrderByDescending(x => x.CreateDate).Include(x => x.BlogComments).Include(x => x.BlogCategory).Where(x => x.IsDeleted == false).Take(3).ToList()
            };
            return View(blogVM);
        }
        [HttpGet]
        public IActionResult BlogDetail(Guid id)
        {
            Blog blog = _appDbContext.Blogs.Include(x => x.BlogComments).FirstOrDefault(x => x.Id == id);
            List<Partner> partners = _appDbContext.Partners.ToList();
            if (blog == null) return View("Error");
            BlogComment blogComment = null;
            blog.ViewCount++;
            BlogViewModel blogVM = new BlogViewModel
            {
                Blog = blog,
                BlogComments = _appDbContext.BlogComments.Where(x => x.BlogId == blog.Id).ToList(),
                RecentBlogComment = _appDbContext.BlogComments.OrderByDescending(x => x.MessageTime).Where(x=>x.BlogId==blog.Id).Where(x => x.IsDeleted == false).Take(5).ToList(),
                BlogCategories = _appDbContext.BlogCategories.Include(x => x.Blogs).Where(x => x.IsDeleted == false).ToList(),
                BlogComment = blogComment,
                Partners = partners,
                RecentBlogs = _appDbContext.Blogs.OrderByDescending(x=>x.CreateDate).Include(x=>x.BlogComments).Include(x=>x.BlogCategory).Where(x=>x.IsDeleted==false).Take(3).ToList()
            };
            _appDbContext.SaveChanges();
            return View(blogVM);
        }
        [HttpPost]
        public IActionResult BlogDetail(BlogViewModel blogVM)
        {
            Blog blog = _appDbContext.Blogs.Include(x => x.BlogComments).FirstOrDefault(x => x.Id == blogVM.BlogComment.BlogId);
            if (blog == null) return View("Error");
            blogVM.BlogComment.BlogId = blog.Id;
            blogVM.Blog = blog;
            BlogComment comment = blogVM.BlogComment;
            blogVM.BlogComments = _appDbContext.BlogComments.Where(x => x.BlogId == blog.Id).ToList();
            blogVM.RecentBlogComment = _appDbContext.BlogComments.OrderByDescending(x => x.MessageTime).Where(x => x.BlogId == blog.Id).Where(x => x.IsDeleted == false).Take(5).ToList();
            blogVM.BlogCategories = _appDbContext.BlogCategories.Include(x => x.Blogs).Where(x => x.IsDeleted == false).ToList();
            blogVM.Partners = _appDbContext.Partners.ToList();
            blogVM.RecentBlogs = _appDbContext.Blogs.OrderByDescending(x => x.CreateDate).Include(x => x.BlogComments).Include(x => x.BlogCategory).Where(x => x.IsDeleted == false).Take(3).ToList();
            blogVM.BlogComment = comment;
            if (!ModelState.IsValid) return View(blogVM);
            if (blogVM.BlogComment.CommentEmail is null)
            {
                ModelState.AddModelError("CommentEmail", "Required to fill");
                return View(blogVM);
            }
            if (blogVM.BlogComment.Comment is null || blogVM.BlogComment.Comment.Length < 10)
            {
                ModelState.AddModelError("Comment", "Required to fill");
                return View(blogVM);
            }
            if (blogVM.BlogComment.FullName is null)
            {
                ModelState.AddModelError("Fullname", "Required to fill");
                return View(blogVM);
            }
            _appDbContext.BlogComments.Add(comment);
            _appDbContext.SaveChanges();
            return RedirectToAction("blogdetail");
        }
    }
}
