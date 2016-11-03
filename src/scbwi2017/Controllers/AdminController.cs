using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using scbwi2017.Data;
using scbwi2017.Models;
using scbwi2017.Models.Data;

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

        public IActionResult Index() => View();

        public IActionResult RegTypes() => Json(_db.Types.ToList());

        [HttpPost]
        public IActionResult RegTypes([FromBody] RegistrationType newreg)
        {
            if (!ModelState.IsValid)
            {
                return Json(new {success = false});
            }

            return Json(new {success = true});
        }

        public IActionResult Comprehensives()
            => Json(_db.Extras.Where(x => x.comprehensive == ExtraType.Comprehensive).ToList());

        [HttpPost]
        public IActionResult Comprehensives([FromBody] Extra c)
        {
            if (!ModelState.IsValid)
            {
                return Json(new {success = false});
            }

            return Json(new {success = true});
        }
        public IActionResult Meals()
            => Json(_db.Meals.ToList());

        [HttpPost]
        public IActionResult Meals([FromBody] Meal m)
        {
            if (!ModelState.IsValid)
            {
                return Json(new {success = false});
            }

            return Json(new {success = true});
        }
    }
}