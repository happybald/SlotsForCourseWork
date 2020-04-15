using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SlotsForCourseWork.ViewModels;
using SlotsForCourseWork.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections;
using System.Runtime.ExceptionServices;
using System;
using System.Web;
using System.Collections.Generic;


namespace CustomIdentityApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        [TempData]
        public string StatusMessage { get; set; }

        public AccountController(ApplicationContext db, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.UserName, Credits = 20 };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(WithdrawModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await this._context.Users.FirstOrDefaultAsync(u => u.Id == _userManager.GetUserAsync(HttpContext.User).Result.Id);
                    user.Credits += (int)model.Value;
                    this._context.Update(user);
                    await this._context.SaveChangesAsync();
                    return Json(new { Message = HttpUtility.JavaScriptStringEncode("Successfully! Your new balance :" + user.Credits, false), NewCredits = user.Credits});
                }
                catch (Exception ex)
                {
                    return Json(new string(ex.Message));

                }
            }
                return Json(new string("Model is not valid!"));

        }

        public IEnumerable Get()
        {
            var users = this._context.Users.OrderByDescending(u => u.Credits).Take(10).ToList<User>();
            List<UsersNormalizedForJSON> usersNormalized = new List<UsersNormalizedForJSON>();
            for (int i = 0; i < users.Count; i++)
            {
                usersNormalized.Add(new UsersNormalizedForJSON(users[i]));
            }
            if (users.Count <= 0)
            {
                return "Error!";
            }
            return usersNormalized;
        }
    }
}