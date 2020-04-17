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

namespace SlotsForCourseWork.Controllers
{
    public class SpinController : Controller
    {

        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public SpinController(ApplicationContext db, UserManager<User> userManager, SignInManager<User> signInManager)
        {
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
                if (_signInManager.IsSignedIn(HttpContext.User))
                {
                    if (_userManager.GetUserAsync(HttpContext.User).Result.Credits > model.Bet)
                    {
                        var user = await this._context.Users.FirstOrDefaultAsync(u => u.Id == _userManager.GetUserAsync(HttpContext.User).Result.Id);
                        user.Credits -= model.Bet;
                        Random random = new Random();
                        int[] randomResult = new int[] { random.Next(0, 4), random.Next(0, 4), random.Next(0, 4), random.Next(0, 4) };
                        int[] counts = new int[] { CountCards(randomResult, 0), CountCards(randomResult, 1), CountCards(randomResult, 2), CountCards(randomResult, 3) };
                        int win = WinCheck(counts);
                        switch (win)
                        {
                            case 0:
                                {
                                    this._context.Update(user);
                                    this._context.Transactions.Add(new Transaction { UserName = user.UserName, Bet=model.Bet, Result = -model.Bet, Time=DateTime.Now });
                                    await this._context.SaveChangesAsync();
                                    return Json(new { Win = false, a = randomResult[0], b = randomResult[1], c = randomResult[2], d = randomResult[3], NewCredits = user.Credits });
                                }
                            case 1:
                                {
                                    int winbet = (int)Math.Pow(3, 2) * (model.Bet - 1) + 3;
                                    user.Credits += winbet;
                                    if (winbet > user.BestScore)
                                    {
                                        user.BestScore = winbet;
                                    }
                                    if (user.RefUserName != null)
                                    {
                                        ReferralReward(winbet, user.RefUserName);
                                    }
                                    this._context.Update(user);
                                    this._context.Transactions.Add(new Transaction { UserName = user.UserName, Bet = model.Bet, Result = winbet, Time = DateTime.Now });
                                    await this._context.SaveChangesAsync();
                                    return Json(new { Win = true, a = randomResult[0], b = randomResult[1], c = randomResult[2], d = randomResult[3], NewCredits = user.Credits, NewBestScore = user.BestScore, WinValue = winbet });
                                }
                            case 2:
                                {
                                    int winbet = (int)Math.Pow(5, 2) * (model.Bet - 1) + 5;
                                    user.Credits += winbet;
                                    {
                                        user.BestScore = winbet;
                                    }
                                    if (user.RefUserName != null)
                                    {
                                        ReferralReward(winbet, user.RefUserName);
                                    }
                                    this._context.Update(user);
                                    this._context.Transactions.Add(new Transaction { UserName = user.UserName, Bet = model.Bet, Result = winbet, Time = DateTime.Now });
                                    await this._context.SaveChangesAsync();
                                    return Json(new { Win = true, a = randomResult[0], b = randomResult[1], c = randomResult[2], d = randomResult[3], NewCredits = user.Credits, NewBestScore = user.BestScore, WinValue = winbet });
                                }
                        }
                    }
                    return Json(new { status = "bad", statusMessage = HttpUtility.JavaScriptStringEncode("You need more credits for this bet!", false) });

                }
                else
                {
                    model.Credits -= model.Bet;
                    Random random = new Random();
                    int[] randomResult = new int[] { random.Next(0, 4), random.Next(0, 4), random.Next(0, 4), random.Next(0, 4) };
                    int[] counts = new int[] { CountCards(randomResult, 0), CountCards(randomResult, 1), CountCards(randomResult, 2), CountCards(randomResult, 3) };
                    int win = WinCheck(counts);
                    switch (win)
                    {
                        case 0:
                            {
                                return Json(new { Win = false, a = randomResult[0], b = randomResult[1], c = randomResult[2], d = randomResult[3], NewCredits = model.Credits });
                            }
                        case 1:
                            {
                                int winbet = (int)Math.Pow(3, 2) * (model.Bet - 1) + 3;
                                model.Credits += winbet;
                                if (winbet > model.BestScore)
                                {
                                    model.BestScore = winbet;
                                }
                                return Json(new { Win = true, a = randomResult[0], b = randomResult[1], c = randomResult[2], d = randomResult[3], NewCredits = model.Credits, NewBestScore = model.BestScore, WinValue = winbet });
                            }
                        case 2:
                            {
                                int winbet = (int)Math.Pow(5, 2) * (model.Bet - 1) + 5;
                                model.Credits += winbet;
                                {
                                    model.BestScore = winbet;
                                }
                                return Json(new { Win = true, a = randomResult[0], b = randomResult[1], c = randomResult[2], d = randomResult[3], NewCredits = model.Credits, NewBestScore = model.BestScore, WinValue = winbet });
                            }
                    }
                }
            }
            return Json(new { status = "bad", statusMessage = HttpUtility.JavaScriptStringEncode("Bad Model", false) });
        }
        private void ReferralReward (int value, string RefUserNameStr)
        {
            var refUser = this._context.Users.FirstOrDefault(u => u.UserName == RefUserNameStr);
            double reward = (Convert.ToDouble(value) / 100) * 5;
            refUser.Credits += (int)reward;
            this._context.Update(refUser);
            if (refUser.RefUserName != null)
            {
                ReferralReward(value, refUser.RefUserName);
            }
            }
        private int WinCheck(int[] cards)
        {
            if (cards[0] == 4 || cards[1] == 4 || cards[2] == 4 || cards[3] == 4)
            {
                return 2;
            }
            if (cards[0] == 2 && cards[1] == 2 || cards[0] == 2 && cards[2] == 2 || cards[0] == 2 && cards[3] == 2 || cards[1] == 2 && cards[2] == 2 || cards[1] == 2 && cards[3] == 2 || cards[2] == 2 && cards[3] == 2)
            {
                return 1;
            }
            return 0;
        }
        private int CountCards(int[] mass, int n)
        {
            int counter = 0;
            for (int i = 0; i < 4; i++)
            {
                if (mass[i] == n)
                {
                    counter++;
                }
            }
            return counter;
        }
    }
}