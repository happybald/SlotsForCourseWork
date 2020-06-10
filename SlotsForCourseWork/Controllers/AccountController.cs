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
using Microsoft.AspNetCore.Authentication;
using System.Web;
using System.Collections.Generic;


namespace CustomIdentityApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;


        public AccountController(ApplicationContext db, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    Credits = 40,
                    BestScore = 0,
                    RefUserName = model.RefUserName
                };
                if (user.UserName == user.RefUserName)
                {
                    return Json(new
                    {
                        status = false,
                        statusMessage = HttpUtility.JavaScriptStringEncode("Referral user incorrect!", false)
                    });
                }
                if (model.RefUserName != null)
                {
                    var refuser = _context.Users.FirstOrDefault(u => u.UserName == model.RefUserName);
                    if (refuser == null)
                    {
                        return Json(new
                        {
                            status = false,
                            statusMessage = HttpUtility.JavaScriptStringEncode("Referral user does not exist!", false)
                        });
                    }
                    refuser.Credits += 200;
                    _context.Update(refuser);
                    await _context.SaveChangesAsync();
                }
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return Json(new
                    {
                        status = true,
                        statusMessage = HttpUtility.JavaScriptStringEncode("Good!")
                    });
                }
                else
                {
                    List<string> resmessage = new List<string>();
                    foreach (var error in result.Errors)
                    {
                        resmessage.Add(error.Code);
                    }
                    return Json(new
                    {
                        status = false,
                        statusMessage = HttpUtility.JavaScriptStringEncode(string.Join(",", resmessage.ToArray()), false)
                    });
                }
            }
            return Json(new
            {
                status = false,
                statusMessage = HttpUtility.JavaScriptStringEncode("Error! Bad Model!")
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return Json(new
                        {
                            status = true,
                            statusMessage = HttpUtility.JavaScriptStringEncode("Good! Your are logined!")
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        status = false,
                        statusMessage = HttpUtility.JavaScriptStringEncode("Incorrect login or pass!")
                    });
                }
            }
            return Json(new
            {
                status = false,
                statusMessage = HttpUtility.JavaScriptStringEncode("Fill all fields!")
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}