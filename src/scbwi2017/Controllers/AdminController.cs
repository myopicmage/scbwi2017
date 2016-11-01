using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using scbwi2017.Data;
using scbwi2017.Models;

namespace scbwi2017.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;


        public AdminController(ApplicationDbContext ctx, UserManager<ApplicationUser> usermgr)
        {
            _db = ctx;
            _userManager = usermgr;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}