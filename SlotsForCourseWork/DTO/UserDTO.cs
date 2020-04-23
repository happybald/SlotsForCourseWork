using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SlotsForCourseWork.DTO
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public int BestScore { get; set; }
    }
}
