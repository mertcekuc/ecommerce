using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace EcommerceApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly EcommerceContext _context;
        private readonly UserManager<User> _userManager;

        public ProductsController(EcommerceContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var products = _context.Products.Include(p => p.Category).ToList();
            return View(products);
        }

        public IActionResult Details(int id) {
            //
            var product = _context.Products.Find(id);
            product.Comments = _context.Comments.Where(c => c.ProductId == id).Include(c => c.User).ToList();

            
            return View(product);
        }
        
        public IActionResult Create() {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
            
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create([Bind("Name,Description")] Product product)
        {
            if (_userManager.GetUserId(User) == null)
                return RedirectToAction("Login", "Users");

            int userId = Convert.ToInt32(_userManager.GetUserId(User));
            product.Price = Convert.ToDecimal(Request.Form["Price"]);
            product.CreateDate = DateTime.Now;
            product.CreateUserId = userId;
            product.CategoryId = Convert.ToInt32(Request.Form["CategoryId"]);

            var file = Request.Form.Files["img"];

            _context.Products.Add(product);
            _context.SaveChanges();

            if (file != null && file.Length > 0)
            {
                
                var directoryPath = Path.Combine("wwwroot", "img", "Products");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Add a file extension (like .jpg, .png) to the file name
                var fileExtension = Path.GetExtension(file.FileName);
                var filePath = Path.Combine(directoryPath, $"{product.Id}{fileExtension}");

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id) {
            return View(_context.Products.Find(id));
        }

        [HttpPost]
        public IActionResult Delete()
        {
            int id = Convert.ToInt32(Request.Form["Id"]);

            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);

            var cartDetails = _context.CartDetails.Where(cd => cd.ProductId == id);
            _context.CartDetails.RemoveRange(cartDetails);

            _context.SaveChanges();

            var directoryPath = Path.Combine("wwwroot", "img", "Products");
            string[] extensions = { ".jpg", ".jpeg", ".png", ".gif" };

            foreach (var ext in extensions)
            {
                var filePath = Path.Combine(directoryPath, $"{id}{ext}");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id) {
            ViewBag.Categories = _context.Categories.ToList();
            return View(_context.Products.Find(id));
        }

        [HttpPost]
        public IActionResult Edit(Product model)
        {
            if (model == null || model.Id == 0)
            {
                return NotFound();
            }

            var product = _context.Products.Find(model.Id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = model.Name;
            product.Price = model.Price;
            product.Description = model.Description;
            product.ModifiedDate = DateTime.Now;

            if (model.CategoryId > 0)
            {
                product.CategoryId = model.CategoryId;
            }

            if (model.ModifiedUserId > 0)
            {
                product.ModifiedUserId = model.ModifiedUserId;
            }

            _context.Products.Update(product);
            _context.SaveChanges();

            // Handle the file upload
            var file = Request.Form.Files["img"];
            if (file != null && file.Length > 0)
            {
                var directoryPath = Path.Combine("wwwroot", "img", "Products");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var fileExtension = Path.GetExtension(file.FileName);
                var filePath = Path.Combine(directoryPath, $"{product.Id}{fileExtension}");

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }

            return RedirectToAction("Edit", new { id = product.Id });

        }

        [HttpPost]
        public IActionResult AddComment()
        {
            int userId = _userManager.GetUserId(User) == null ? 0 : int.Parse(_userManager.GetUserId(User));
            if (userId == 0)
                return RedirectToAction("Login", "Users");
            Comment comment = new Comment
            {
                ProductId = Convert.ToInt32(Request.Form["ProductId"]),
                UserId = userId,
                Comment1 = Request.Form["Content"],
                CreatedDate = DateTime.Now
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();
            return RedirectToAction("Details", new { id = comment.ProductId });
        }


    }
}
