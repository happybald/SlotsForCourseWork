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
using SlotsForCourseWork.Services.Contracts;
using SlotsForCourseWork.DTO;
using System.ComponentModel;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace SlotsForCourseWork.Controllers
{
    public class SpinController : Controller
    {

        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ISpinService _spinService;
        private readonly SignInManager<User> _signInManager;

        public SpinController(ApplicationContext db, UserManager<User> userManager, SignInManager<User> signInManager, ISpinService spinService)
        {
            _spinService = spinService;
            _context = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // POST: SpinStart/Create
        [HttpPost]
        public async Task<IActionResult> Start(SpinViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Credits< model.Bet)
                {
                    return Json(new { status = "bad", statusMessage = HttpUtility.JavaScriptStringEncode("You need more credits for this bet!", false) });
                }
                if (_signInManager.IsSignedIn(HttpContext.User))
                {
                    if (this._userManager.GetUserAsync(User).Result.Credits < model.Bet)
                    {
                        return Json(new { status = "bad", statusMessage = HttpUtility.JavaScriptStringEncode("You need more credits for this bet!", false) });
                    }
                    var user = await this._userManager.GetUserAsync(User);
                    return Json(await this._spinService.StartUser(model, user));
                }
                else
                {
                    return Json(await this._spinService.StartGuest(model));
                }
            }
            return Json(new { status = "bad", statusMessage = HttpUtility.JavaScriptStringEncode("Bad Model", false) });
        }
    }
}