using SlotsForCourseWork.Models;
using SlotsForCourseWork.Services.Contracts;
using SlotsForCourseWork.Services.Exceptions;
using SlotsForCourseWork.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using SlotsForCourseWork.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Web;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace SlotsForCourseWork.Services
{
    public class SpinService : ISpinService
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITransactionService _transactionService;

        public SpinService(ApplicationContext context, UserManager<User> userManager, SignInManager<User> signInManager, ITransactionService transactionService)
        {
            this._transactionService = transactionService ?? throw new ServiceException(nameof(transactionService));
            this._context = context ?? throw new ServiceException(nameof(context));
            this._signInManager = signInManager ?? throw new ServiceException(nameof(signInManager));
            this._userManager = userManager ?? throw new ServiceException(nameof(userManager));
        }


        public async Task<ResultDTO> StartGuest(SpinViewModel model)
        {
            SpinDTO result;
            int win;
                result = this.Spin();
                win = this.ToWin(result, model);
            if (win > 0)
            {
                if (win > model.BestScore)
                {
                    return new ResultDTO { win = true, winValue = win, a = result.a, b = result.b, c = result.c, d = result.d, newBestScore = win,newCredits=model.Credits+win };
                }
                return new ResultDTO { win = true, winValue = win, a = result.a, b = result.b, c = result.c, d = result.d ,newCredits=model.Credits+win};
            }
            else
            {
                return new ResultDTO { win = false, winValue = win, a = result.a, b = result.b, c = result.c, d = result.d,newCredits=model.Credits+win };
            }
        }


        public async Task<ResultDTO> StartUser(SpinViewModel model, User user)
        {
            user.Credits -= model.Bet;
            this._context.CasinoInfo.FirstOrDefault(c => c.id == 1).CasinoCash += model.Bet;
            SpinDTO result;
            int win;
            do
            {
                result = this.Spin();
                win = this.ToWin(result, model);
            } while (this._context.CasinoInfo.FirstOrDefault(c => c.id == 1).CasinoCash - win < Constants.CASINO_MONEY);
            await this._transactionService.AddTransactionAsync(user.UserName, model.Bet, win);
            if (win > 0)
            {
                user.Credits += win;
                this._context.CasinoInfo.FirstOrDefault(c => c.id == 1).CasinoCash -= win;
                if(user.RefUserName != null)
                {
                    ReferralReward(win, user.RefUserName);
                }
                if (win > user.BestScore)
                {
                    user.BestScore = win;
                    this._context.Update(user);
                    await this._context.SaveChangesAsync();
                    return new ResultDTO{ win = true, winValue = win, a = result.a, b = result.b, c = result.c, d = result.d, newBestScore = win,newCredits=user.Credits};
                }
                this._context.Update(user);
                await this._context.SaveChangesAsync();
                return new ResultDTO { win = true, winValue = win, a = result.a, b = result.b, c = result.c, d = result.d,newCredits=user.Credits};
            }
            else
            {
                await this._context.SaveChangesAsync();
                return new ResultDTO{ win = false, winValue = win, a = result.a, b = result.b, c = result.c, d = result.d,newCredits=user.Credits};
            }
        }

        private SpinDTO Spin()
        {
            Random rnd = new Random();
            SpinDTO temp = new SpinDTO();
            temp.a = rnd.Next(1, 4);
            temp.b = rnd.Next(1, 4);
            temp.c = rnd.Next(1, 4);
            temp.d = rnd.Next(1, 4);
            temp.WinType = WinCheck(temp);

            return temp;

        }
        private int ToWin(SpinDTO spin, SpinViewModel model)
        {
            int win = 0;
            switch (spin.WinType)
            {
                case 1:
                    {
                        win = (int)Math.Pow(4, 2) * (model.Bet - 1) + 4;
                        break;
                    }
                case 2:
                    {
                        win = (int)Math.Pow(2, 2) * (model.Bet - 1) + 2;
                        break;
                    }
                default:
                    {
                        win = -model.Bet;
                        break;
                    }
            }
            return win;
        }
        private int WinCheck(SpinDTO spin)
        {
            int a = 0, b = 0, c = 0, d = 0;
            int[] slotsArray = { spin.a, spin.b, spin.c, spin.d };
            for (int i = 0; i < 4; i++)
            {
                if (slotsArray[i] == 1)
                {
                    a++;
                }
                if (slotsArray[i] == 2)
                {
                    b++;
                }
                if (slotsArray[i] == 3)
                {
                    c++;
                }
                if (slotsArray[i] == 4)
                {
                    d++;
                }
            }
            if (a == 4 || b == 4 || c == 4 | d == 4)
            {
                return 1;
            }
            if ((a == 2 && b == 2) || (a == 2 && c == 2) || (a == 2 && d == 2) || (b == 2 && c == 2) || (b == 2 & d == 2) || (c == 2 && d == 2))
            {
                return 2;
            }
            return 0;
        }

        private void ReferralReward(int value, string RefUserNameStr)
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
    }
}
