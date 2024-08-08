using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Identity;
using EcommerceApp.Models.ViewModels.Auth;

namespace EcommerceApp.Controllers
{
    public class UsersController : Controller
    {
        readonly UserManager<User> _userManager;
        readonly SignInManager<User> _signInManager;

        private readonly EcommerceDbContext _context;
        private readonly EcommerceContext _ecommerceContext;

        public UsersController(EcommerceContext ecommerceContext, EcommerceDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _ecommerceContext = ecommerceContext;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public IActionResult Index()
        {
            return View(_context.Users.ToList());
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                User appUser = new User
                {
                    UserName = userViewModel.UserName,
                    Email = userViewModel.Email,
                    LastName = userViewModel.LastName,
                    FirstName = userViewModel.FirstName,
                    CreatedDate = DateTime.Now
                };

                IdentityResult result = await _userManager.CreateAsync(appUser, userViewModel.Password);
                if (result.Succeeded)
                {
                    var cart = new Cart()
					{
						UserId = appUser.Id
					};
                    _ecommerceContext.Carts.Add(cart);
                    _ecommerceContext.SaveChanges();
                    // Sign in the user
                    await _signInManager.SignInAsync(appUser, isPersistent: false);
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Add errors to ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(userViewModel);
        }


        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignInViewModel signInViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(signInViewModel.UserName, signInViewModel.Password, signInViewModel.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            return View(signInViewModel);
        }

        
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
            
        }


        // GET: Users/Details/5
        public IActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var user = _context.Users
            //    .First(m => m.Id == id);
            //if (user == null)
            //{
            //    return NotFound();
            //}

            return View();
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Create([Bind("Email,UserName,Name,LastName")] User user)
        {
            if (ModelState.IsValid)
            {   
                //user.CreatedDate = DateTime.Now;
                _context.Add(user);
                _context.SaveChanges();
                var cart = new Cart()
                {
                    //userId = user.Id,
				};
                _context.Add(cart);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user =  _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        public IActionResult Edit(int id)
        {
           
            //var user = _context.Users.Find(id);
            //user.LastName = Request.Form["LastName"];
            //user.Name = Request.Form["Name"];
            //user.UserName = Request.Form["UserName"];
           
            //user.ModifiedDate = DateTime.Now;
            //_context.Update(user);
            //_context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.Users
                .Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult  Delete(int id)
        {
            
                if (id == null)
                {
                    return NotFound();
                }

                User user;
                try
                {
                    user = _context.Users
                       .Find(id);
                }
                catch
                {
                    return NotFound();
                }

                _context.Users.Remove(user);
                _context.SaveChanges();

                
            
            return RedirectToAction(nameof(Index));
            
        }


    }
}
