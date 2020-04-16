using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SlotsForCourseWork.Models
{
    public class User : IdentityUser
    {
        public int Credits { get; set; }
        public int BestScore { get; set; }
    }
}
