using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using scbwi2017.Data;
using scbwi2017.Models;
using scbwi2017.Models.Data;

namespace scbwi2017.Controllers
{
    [Authorize()]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AdminController(ApplicationDbContext ctx, UserManager<ApplicationUser> usermgr, RoleManager<IdentityRole> rm)
        {
            _db = ctx;
            _userManager = usermgr;
            _roleManager = rm;
        }

        public IActionResult Index() => View();

        public IActionResult RegTypes() => Json(_db.Types.ToList());

        public async Task<IActionResult> MakeAdmin(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return Json(new {message = "not found"});

            var adminrole = _roleManager.Roles.SingleOrDefault(x => x.Name == "Admin");

            if (adminrole == null)
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var result = await _userManager.AddToRoleAsync(user, "Admin");

            return Json(result);
        }

        [HttpPost]
        public IActionResult RegTypes([FromBody] RegistrationType newreg)
        {
            if (!ModelState.IsValid)
            {
                return Json(new {success = false});
            }

            newreg.created = DateTime.Now;
            newreg.createdby = User.Identity.Name;
            newreg.modified = DateTime.Now;
            newreg.modifiedby = User.Identity.Name;

            if (newreg.friday)
            {
                newreg.saturday = false;
                newreg.sunday = false;
            }
            else if (newreg.allowsworkshops == false)
            {
                newreg.friday = false;
                newreg.sunday = false;
            }

            try
            {
                _db.Types.Add(newreg);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new {success = false, message = $"db error: {ex.Message}"});
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

            c.created = DateTime.Now;
            c.modified = DateTime.Now;
            c.createdby = User.Identity.Name;
            c.modifiedby = User.Identity.Name;
            c.extracost = 0;
            c.comprehensive = ExtraType.Comprehensive;

            try
            {
                _db.Extras.Add(c);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new {success = false, message = $"db error: {ex.Message}"});
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

            m.created = DateTime.Now;
            m.modified = DateTime.Now;
            m.createdby = User.Identity.Name;
            m.modifiedby = User.Identity.Name;
            m.mealtype = MealType.Dinner;

            try
            {
                _db.Meals.Add(m);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new {success = false, message = $"db error: {ex.Message}"});
            }

            return Json(new {success = true});
        }
        public IActionResult Workshops()
            => Json(_db.Workshops.ToList());

        [HttpPost]
        public IActionResult Workshops([FromBody] Workshop w)
        {
            if (!ModelState.IsValid)
            {
                return Json(new {success = false});
            }

            w.created = DateTime.Now;
            w.modified = DateTime.Now;
            w.createdby = User.Identity.Name;
            w.modifiedby = User.Identity.Name;

            try
            {
                _db.Workshops.Add(w);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new {success = false, message = $"db error: {ex.Message}"});
            }

            return Json(new {success = true});
        }

        public IActionResult Coupons()
            => Json(_db.Coupons.ToList());

        [HttpPost]
        public IActionResult Coupons([FromBody] Coupon c)
        {
            if (!ModelState.IsValid)
            {
                return Json(new {success = false});
            }

            c.created = DateTime.Now;
            c.modified = DateTime.Now;
            c.createdby = User.Identity.Name;
            c.modifiedby = User.Identity.Name;

            try
            {
                _db.Coupons.Add(c);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new {success = false, message = $"db error: {ex.Message}"});
            }

            return Json(new {success = true});
        }

        public IActionResult Prices()
        {
            var types = "satdinner,portfolio,manuscript";

            var prices = _db.Prices
                .Where(x => types.Contains(x.type))
                .Select(x => new { x.type, x.value })
                .ToArray();

            return Json(prices);
        }

        [HttpPost]
        public IActionResult Prices([FromBody] Price p)
        {
            if (!ModelState.IsValid)
            {
                return Json(new {success = false});
            }

            var oldp = _db.Prices.SingleOrDefault(x => x.type == p.type);

            if (oldp == null)
            {
                return Json(new {success = false, message = "old price not found"});
            }

            oldp.value = p.value;
            oldp.modifiedby = User.Identity.Name;
            oldp.modified = DateTime.Now;

            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new {success = false, message = $"db error: {ex.Message}"});
            }

            return Json(new {success = true});
        }

        public IActionResult Late() => Json(_db.Dates.SingleOrDefault(x => x.name == "late"));

        [HttpPost]
        public IActionResult Late([FromBody] DateTime d)
        {
            if (!ModelState.IsValid)
            {
                return Json(new {success = false});
            }

            var old = _db.Dates.SingleOrDefault(x => x.name == "late");

            if (old == null)
            {
                return Json(new {success = false, message = $"Late date not found, application error"});
            }

            old.value = d;

            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new {success = false, message = $"db error: {ex.Message}"});
            }

            return Json(new {success = true});
        }
    }
}