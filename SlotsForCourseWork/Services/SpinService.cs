using SlotsForCourseWork.Models;
using SlotsForCourseWork.Services.Contracts;
using SlotsForCourseWork.Services.Exceptions;
using SlotsForCourseWork.DTO;
using System;
using SlotsForCourseWork.ViewModels;
using System.Linq;

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


        public ResultDto StartGuest(SpinViewModel model)
        {
            var result = Spin();
            var win = ToWin(result, model);
            if (win <= 0) return new ResultDto(result, win, model);
            if (win > model.BestScore)
            {
                model.BestScore = win;
            }

            return new ResultDto(result, win, model);
        }


        public ResultDto StartUser(SpinViewModel model, User user)
        {
            user.Credits -= model.Bet;
            var casinoInfo = _context.CasinoInfo.FirstOrDefault(c => c.Id == Constants.CasinoId);
            casinoInfo.CasinoCash += model.Bet;
            SpinDto result;
            int win;
            do
            {
                result = Spin();
                win = ToWin(result, model);
            } while (casinoInfo.CasinoCash - win < Constants.CasinoMoneylim);

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
            return new ResultDto(result, win, user);
        }

        private SpinDto Spin()
        {
            var rnd = new Random();
            var temp = new SpinDto(rnd.Next(1, 4), rnd.Next(1, 4), rnd.Next(1, 4), rnd.Next(1, 4));
            temp.WinType = WinCheck(temp);
            return temp;
        }

        private int ToWin(SpinDto spin, SpinViewModel model)
        {
            return spin.WinType switch
            {
                1 => (int) Math.Pow(4, 2) * (model.Bet - 1) + 4,
                2 => (int) Math.Pow(2, 2) * (model.Bet - 1) + 2,
                _ => -model.Bet
            };
        }

        private int WinCheck(SpinDto spin)
        {
            int a = 0, b = 0, c = 0, d = 0;
            int[] slotsArray = {spin.A, spin.B, spin.C, spin.D};
            for (var i = 0; i < 4; i++)
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

        private void ReferralReward(int value, string refUserNameStr)
        {
            var refUser = _context.Users.FirstOrDefault(u => u.UserName == refUserNameStr);
            var reward = (Convert.ToDouble(value) / 100) * 5;
            if (refUser != null)
            {
                refUser.Credits += (int) reward;
                _context.Update(refUser);
                if (refUser.RefUserName != null)
                {
                    ReferralReward(value, refUser.RefUserName);
                }
            }
        }
    }
}