using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SlotsForCourseWork.Models
{
    public class UsersNormalizedForJSON
    {
        public string Id { get; set; }
        public int Credits { get; set; }
        public string UserName { get; set; }

        public UsersNormalizedForJSON(User p)
        {
            Id = p.Id;
            Credits = p.Credits;
            UserName = p.UserName;
        }
    }
}
