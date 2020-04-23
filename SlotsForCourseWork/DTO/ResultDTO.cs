using SlotsForCourseWork.Models;
using SlotsForCourseWork.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlotsForCourseWork.DTO
{
    public class ResultDTO
    {
        public bool win { get; set; }
        public int a { get; set; }
        public int b { get; set; }
        public int c { get; set; }
        public int d { get; set; }
        public int newBestScore { get; set; }
        public int winValue { get; set; }
        public int newCredits { get; set; }
    public ResultDTO(SpinDTO spin, int win, User user)
    {
            this.a = spin.a;
            this.b = spin.a;
            this.c = spin.a;
            this.d = spin.a;
            this.newBestScore = user.BestScore;
            this.newCredits = user.Credits;
            this.win = (win > 0) ? true : false;
            this.winValue = win;
        }
        public ResultDTO(SpinDTO spin, int win,SpinViewModel model)
        {
            this.a = spin.a;
            this.b = spin.a;
            this.c = spin.a;
            this.d = spin.a;
            this.newBestScore = model.BestScore;
            this.newCredits = model.Credits;
            this.win = (win > 0) ? true : false;
            this.winValue = win;
        }
    }
}
