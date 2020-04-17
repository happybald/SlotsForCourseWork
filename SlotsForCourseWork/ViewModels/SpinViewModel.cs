using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SlotsForCourseWork.ViewModels
{
    public class SpinViewModel
    {
        [Required]
        public int Bet { get; set; }

        public int Credits { get; set; }
        public int BestScore { get; set; }

    }
}
