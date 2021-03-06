﻿using SlotsForCourseWork.Models;
using SlotsForCourseWork.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlotsForCourseWork.DTO
{
    public class ResultDto
    {
        public bool IsWin => WinValue > 0;
        public int A { get; }
        public int B { get; }
        public int C { get; }
        public int D { get; }
        public int NewBestScore { get; }
        public int WinValue { get; }
        public int NewCredits { get; }

        public ResultDto(SpinDto spin, int win, User user)
        {
            A = spin.A;
            B = spin.B;
            C = spin.C;
            D = spin.D;
            NewBestScore = user.BestScore;
            NewCredits = user.Credits;
            WinValue = win;
        }
        public ResultDto(SpinDto spin, int win, SpinViewModel model)
        {
            A = spin.A;
            B = spin.B;
            C = spin.C;
            D = spin.D;
            NewBestScore = model.BestScore;
            NewCredits = model.Credits;
            WinValue = win;
        }
    }
}
