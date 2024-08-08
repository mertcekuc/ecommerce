using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceApp.Models;

namespace EcommerceApp.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly EcommerceContext _context;

        public CategoriesController(EcommerceContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            return View(_context.Categories.ToList());
        }
      
        public IActionResult Details(int id) {
            var category = _context.Categories.Find(id);
            return View(category);
        }

        public IActionResult Edit(int id) {
            return View(_context.Categories.Find(id));
        }
        [HttpPost]
        public IActionResult Edit()
        {   
            int id = Convert.ToInt32(Request.Form["Id"]);
            var category = _context.Categories.Find(id);
            if (category == null) {
                return NotFound();
            }
            category.Name = Request.Form["Name"];
            category.Description = Request.Form["Description"];
            category.ModifiedDate = DateTime.Now;
            category.ModifiedUserId = 1;
            category.Active = Request.Form["Active"].Contains("true");
            
            _context.Categories.Update(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id) {
            return View(_context.Categories.Find(id));
        }

        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public IActionResult Create([Bind("Name,Description")] Category category)
        {

            category.CreateDate = DateTime.Now;
            category.CreateUserId = 1;
            category.Active = Request.Form["Active"].Contains("true");
           

            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
