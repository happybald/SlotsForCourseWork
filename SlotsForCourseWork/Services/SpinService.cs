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
        private readonly ITransactionService _transactionService;

        public SpinService(ApplicationContext context, ITransactionService transactionService)
        {
            _transactionService = transactionService ?? throw new ServiceException(nameof(transactionService));
            _context = context ?? throw new ServiceException(nameof(context));
        }


        public ResultDTO StartGuest(SpinViewModel model)
        {
            SpinDTO result = Spin();
            int win = ToWin(result, model);
            if (win > 0)
            {
                if (win > model.BestScore)
                {
                    model.BestScore = win;
                }
            }
            return new ResultDTO(result, win, model);
        }


        public ResultDTO StartUser(SpinViewModel model, User user)
        {
            user.Credits -= model.Bet;
            var casinoInfo = _context.CasinoInfo.FirstOrDefault(c => c.Id == Constants.CASINO_ID);
            casinoInfo.CasinoCash += model.Bet;
            SpinDTO result;
            int win;
            do
            {
                result = Spin();
                win = ToWin(result, model);
            } while (casinoInfo.CasinoCash - win < Constants.CASINO_MONEYLIM);

            _transactionService.AddTransaction(user.UserName, model.Bet, win);
            if (win > 0)
            {
                user.Credits += win;
                casinoInfo.CasinoCash -= win;
                if (user.RefUserName != null)
                {
                    ReferralReward(win, user.RefUserName);
                }
                if (win > user.BestScore)
                {
                    user.BestScore = win;
                }
            }
            _context.Update(user);
            _context.SaveChangesAsync();
            return new ResultDTO(result, win, user);
        }

        private SpinDTO Spin()
        {
            Random rnd = new Random();
            SpinDTO temp = new SpinDTO(rnd.Next(1, 4), rnd.Next(1, 4), rnd.Next(1, 4), rnd.Next(1, 4));
            temp.WinType = WinCheck(temp);
            return temp;
        }

        private int ToWin(SpinDTO spin, SpinViewModel model)
        {
            switch (spin.WinType)
            {
                case 1:
                    {
                        return (int)Math.Pow(4, 2) * (model.Bet - 1) + 4;
                    }
                case 2:
                    {
                        return (int)Math.Pow(2, 2) * (model.Bet - 1) + 2;
                    }
                default:
                    {
                        return -model.Bet;
                    }
            }
        }

        private int WinCheck(SpinDTO spin)
        {
            int a = 0, b = 0, c = 0, d = 0;
            int[] slotsArray = { spin.A, spin.B, spin.C, spin.D };
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
            if ((a == 2 && b == 2) || (a == 2 && c == 2) || (a == 2 && d == 2) ||
                (b == 2 && c == 2) || (b == 2 && d == 2) || (c == 2 && d == 2))
            {
                return 2;
            }
            return 0;
        }

        private void ReferralReward(int value, string RefUserNameStr)
        {
            var refUser = _context.Users.FirstOrDefault(u => u.UserName == RefUserNameStr);
            double reward = (Convert.ToDouble(value) / 100) * 5;
            refUser.Credits += (int)reward;
            _context.Update(refUser);
            if (refUser.RefUserName != null)
            {
                ReferralReward(value, refUser.RefUserName);
            }
        }
    }
}
