using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using SlotsForCourseWork.Models;

namespace SlotsForCourseWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CredtiAPIController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;

        public CredtiAPIController(ApplicationContext db, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = db;
        }

        // GET: api/CredtiAPI
        [HttpGet]
        public IEnumerable Get()
        {
            var users = this._context.Users.OrderByDescending(u => u.Credits).Take(10).ToList<User>();
            List<UsersNormalizedForJSON> usersNormalized = new List<UsersNormalizedForJSON>();
            for (int i = 0; i < users.Count; i++)
            {
                usersNormalized.Add(new UsersNormalizedForJSON(users[i]));
            }
            if (users.Count <= 0)
            {
                return "Error!";
            }
            return usersNormalized;
        }
    }
}
