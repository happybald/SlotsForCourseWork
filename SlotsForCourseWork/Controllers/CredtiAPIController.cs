﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SlotsForCourseWork.DTO;
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

        public CredtiAPIController(ApplicationContext db)
        {
            _context = db;
        }


        [HttpGet]
        public IEnumerable Get()
        {
            return _context.Users
                .OrderByDescending(t => t.BestScore)
                .Take(10)
                .Select(t => new UserDTO(t.UserName, t.BestScore))
                .ToList();
        }
    }
}
