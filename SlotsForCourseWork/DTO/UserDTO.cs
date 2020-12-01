using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SlotsForCourseWork.DTO
{
    public class UserDto
    {
        public string UserName { get; }
        public int BestScore { get; }

        public UserDto(string userName, int bestScore)
        {
            UserName = userName;
            BestScore = bestScore;
        }
    }
}
