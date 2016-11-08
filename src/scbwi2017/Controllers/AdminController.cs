using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using scbwi2017.Data;
using scbwi2017.Models;
using scbwi2017.Models.Data;
using scbwi2017.Models.RegistrationViewModels;
using scbwi2017.Services;

namespace scbwi2017.Controllers
{
    [Authorize()]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;
        private readonly IEmailSender _msgSender;


        public AdminController(ApplicationDbContext ctx, UserManager<ApplicationUser> usermgr, RoleManager<IdentityRole> rm, ILoggerFactory factory, IEmailSender sender)
        {
            _db = ctx;
            _userManager = usermgr;
            _roleManager = rm;
            _logger = factory.CreateLogger("All");
            _msgSender = sender;
        }

        public IActionResult Index() => View();

        public IActionResult Registration(int id)
        {
            ViewData["id"] = id;

            return View();
        }

        public IActionResult Registrations() => View();

        [HttpPost]
        public IActionResult Registrations(bool frontPage)
        {
            var reg = _db.Registrations
                .OrderBy(x => x.paid)
                .Include(x => x.user)
                .Include(x => x.comprehensive)
                .Include(x => x.first)
                .Include(x => x.second)
                .Include(x => x.coupon)
                .Include(x => x.meal)
                .Include(x => x.type);

            _logger.LogInformation($"Front page: {frontPage}");

            return frontPage ? Json(reg.OrderByDescending(x => x.paid).Select(x => x.Flatten()).Take(10).ToList()) : Json(reg.Select(x => x.Flatten()).ToList());
        }

        [Produces("text/csv")]
        public IActionResult GetCsv()
        {
            var reg = _db.Registrations
                .OrderBy(x => x.paid)
                .Include(x => x.user)
                .Include(x => x.comprehensive)
                .Include(x => x.first)
                .Include(x => x.second)
                .Include(x => x.coupon)
                .Include(x => x.meal)
                .Include(x => x.type)
                .Select(x => x.Flatten())
                .ToList();

            Response.Headers.Add("content-disposition", $"attachment; filename=registrations-{DateTime.Now:s}.csv");

            return Ok(reg);
        }

        [HttpPost]
        public IActionResult GetRegistration([FromBody] int id)
        {
            var reg = _db.Registrations
                .Include(x => x.user)
                .Include(x => x.comprehensive)
                .Include(x => x.first)
                .Include(x => x.second)
                .Include(x => x.coupon)
                .Include(x => x.meal)
                .Include(x => x.type)
                .Include(x => x.comprehensive)
                .SingleOrDefault(x => x.id == id);

            if (reg == null)
            {
                return Json(new { success = false, message = "not found" });
            }

            return Json(new { success = true, data = reg });
        }

        [HttpPost]
        public IActionResult UpdateRegWithoutCharge([FromBody] RegistrationViewModel r)
        {
            var rvm = _db.Registrations.SingleOrDefault(x => x.id == r.id);
            var totals = TotalCalc.CalcTotal(r, _db, _logger);

            var reg = new Registration(r)
            {
                cleared = new DateTime(2000, 1, 1),
                coupon = _db.Coupons.SingleOrDefault(x => x.text == r.coupon),
                created = DateTime.Now,
                createdby = r.user.email,
                first = _db.Workshops.SingleOrDefault(x => x.id == r.first),
                manuscript = r.manuscripts,
                meal = _db.Meals.SingleOrDefault(x => x.id == r.meal),
                modified = DateTime.Now,
                modifiedby = r.user.email,
                paid = new DateTime(2000, 1, 1),
                paypalid = $"{r.user.first}{r.user.last}{DateTime.Now}",
                portfolio = r.portfolios,
                second = _db.Workshops.SingleOrDefault(x => x.id == r.second),
                subtotal = totals.subtotal,
                takingbus = r.takingbus == "true",
                total = totals.total,
                type = _db.Types.SingleOrDefault(x => x.id == r.type),
                satdinner = r.satdinner,
                comprehensive = _db.Extras.SingleOrDefault(x => x.id == r.comprehensive)
            };

            rvm.user = reg.user;
            rvm.cleared = reg.cleared;
            rvm.coupon = reg.coupon;
            rvm.first = reg.first;
            rvm.manuscript = reg.manuscript;
            rvm.meal = reg.meal;
            rvm.modified = DateTime.Now;
            rvm.modifiedby = User.Identity.Name;
            rvm.portfolio = reg.portfolio;
            rvm.second = reg.second;
            rvm.takingbus = reg.takingbus;
            rvm.type = reg.type;
            rvm.satdinner = reg.satdinner;
            rvm.comprehensive = reg.comprehensive;
            rvm.subtotal = totals.subtotal;
            rvm.total = totals.total;

            try
            {
                _db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"db error: {ex.Message}" });
            }
        }

        [HttpPost]
        public IActionResult UpdateRegWithCharge([FromBody] RegistrationViewModel r)
        {
            var rvm = _db.Registrations.SingleOrDefault(x => x.id == r.id);
            var totals = TotalCalc.CalcTotal(r, _db, _logger);

            var reg = new Registration(r)
            {
                cleared = new DateTime(2000, 1, 1),
                coupon = _db.Coupons.SingleOrDefault(x => x.text == r.coupon),
                created = DateTime.Now,
                createdby = r.user.email,
                first = _db.Workshops.SingleOrDefault(x => x.id == r.first),
                manuscript = r.manuscripts,
                meal = _db.Meals.SingleOrDefault(x => x.id == r.meal),
                modified = DateTime.Now,
                modifiedby = r.user.email,
                paid = new DateTime(2000, 1, 1),
                paypalid = $"{r.user.first}{r.user.last}{DateTime.Now}",
                portfolio = r.portfolios,
                second = _db.Workshops.SingleOrDefault(x => x.id == r.second),
                subtotal = totals.subtotal,
                takingbus = r.takingbus == "true",
                total = totals.total,
                type = _db.Types.SingleOrDefault(x => x.id == r.type),
                satdinner = r.satdinner,
                comprehensive = _db.Extras.SingleOrDefault(x => x.id == r.comprehensive)
            };

            rvm.user = reg.user;
            rvm.cleared = reg.cleared;
            rvm.coupon = reg.coupon;
            rvm.first = reg.first;
            rvm.manuscript = reg.manuscript;
            rvm.meal = reg.meal;
            rvm.modified = DateTime.Now;
            rvm.modifiedby = User.Identity.Name;
            rvm.portfolio = reg.portfolio;
            rvm.second = reg.second;
            rvm.takingbus = reg.takingbus;
            rvm.type = reg.type;
            rvm.satdinner = reg.satdinner;
            rvm.comprehensive = reg.comprehensive;

            if (rvm.total != totals.total)
            {
                CreatePending(rvm, reg, totals);
            }

            rvm.subtotal = totals.subtotal;
            rvm.total = totals.total;

            try
            {
                _db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"db error: {ex.Message}" });
            }
        }

        private async void CreatePending(Registration old, Registration newr, Totals t)
        {
            var p = new Pending
            {
                total = t.total - old.total,
                active = true,
                displayid = Guid.NewGuid(),
                message = "You have requested some new charges for your registration:<br />"
            };

            if (old.comprehensive == null && newr.comprehensive != null)
            {
                p.comprehensive = true;
                p.c_name = newr.comprehensive.title;
                p.message += $"Comprehensive: {newr.comprehensive.title}<br />";
            }

            if (newr.manuscript > old.manuscript)
            {
                p.manuscript = newr.manuscript - old.manuscript;
                p.message += $"Manuscript Critiques: New: {newr.manuscript} Old: {old.manuscript}<br />";
            }

            if (newr.portfolio > old.portfolio)
            {
                p.portfolio = newr.portfolio - old.portfolio;
                p.message += $"Portfolio Reviews: New: {newr.portfolio} Old: {old.manuscript}<br />";
            }

            if (newr.satdinner > old.satdinner)
            {
                p.satdinner = newr.satdinner - old.satdinner;
                p.message += $"Saturday Dinners: New: {newr.satdinner} Old: {old.satdinner}<br />";
            }

            p.message +=
                $"<br />If you initiated this request, please go to <a href='https://register.scbwiflorida.com/register/update?id={p.displayid}";
            p.message += "this link</a> and submit your updated payment.<br />";
            p.message += "Thanks,<br />SCBWI Florida Registration Bot";

            try
            {
                _db.SaveChanges();
                var result = await _msgSender.SendEmailAsync(newr.user.Email, "Updated Registration", p.message,
                    newr.user + newr.user.lastname);

                if (!result.IsSuccessStatusCode)
                {
                    _logger.LogCritical("Failed to send email!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to save new charges for {old.id}. {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult DeleteReg(int id)
        {
            var type = _db.Registrations
                .Include(x => x.user)
                .Include(x => x.comprehensive)
                .Include(x => x.first)
                .Include(x => x.second)
                .Include(x => x.coupon)
                .Include(x => x.meal)
                .Include(x => x.type)
                .Include(x => x.comprehensive)
                .SingleOrDefault(x => x.id == id);

            if (type == null)
            {
                return Json(new { success = false });
            }

            _db.RegArchive.Add(type.Flatten());
            _db.Registrations.Remove(type);

            try
            {
                _db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"db error: {ex.Message}"
                });
            }
        }

        public IActionResult RegTypes() => Json(_db.Types.ToList());

        [HttpPost]
        public IActionResult RegTypes([FromBody] RegistrationType newreg)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false });
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
                return Json(new { success = false, message = $"db error: {ex.Message}" });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult DeleteType(int id)
        {
            var type = _db.Types.SingleOrDefault(x => x.id == id);

            if (type == null)
            {
                return Json(new { success = false });
            }

            _db.Types.Remove(type);

            try
            {
                _db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"db error: {ex.Message}"
                });
            }
        }

        public IActionResult Comprehensives()
            => Json(_db.Extras.Where(x => x.comprehensive == ExtraType.Comprehensive).ToList());

        [HttpPost]
        public IActionResult Comprehensives([FromBody] Extra c)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false });
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
                return Json(new { success = false, message = $"db error: {ex.Message}" });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult DeleteComprehensive(int id)
        {
            var type = _db.Extras.SingleOrDefault(x => x.id == id);

            if (type == null)
            {
                return Json(new { success = false });
            }

            _db.Extras.Remove(type);

            try
            {
                _db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"db error: {ex.Message}"
                });
            }
        }

        public IActionResult Meals()
            => Json(_db.Meals.ToList());

        [HttpPost]
        public IActionResult Meals([FromBody] Meal m)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false });
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
                return Json(new { success = false, message = $"db error: {ex.Message}" });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult DeleteMeal(int id)
        {
            var type = _db.Meals.SingleOrDefault(x => x.id == id);

            if (type == null)
            {
                return Json(new { success = false });
            }

            _db.Meals.Remove(type);

            try
            {
                _db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"db error: {ex.Message}"
                });
            }
        }

        public IActionResult Workshops()
            => Json(_db.Workshops.ToList());

        [HttpPost]
        public IActionResult Workshops([FromBody] Workshop w)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false });
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
                return Json(new { success = false, message = $"db error: {ex.Message}" });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult DeleteWorkshop(int id)
        {
            var type = _db.Workshops.SingleOrDefault(x => x.id == id);

            if (type == null)
            {
                return Json(new { success = false });
            }

            _db.Workshops.Remove(type);

            try
            {
                _db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"db error: {ex.Message}"
                });
            }
        }

        public IActionResult Coupons()
            => Json(_db.Coupons.ToList());

        [HttpPost]
        public IActionResult Coupons([FromBody] Coupon c)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false });
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
                return Json(new { success = false, message = $"db error: {ex.Message}" });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult DeleteCoupon(int id)
        {
            var type = _db.Coupons.SingleOrDefault(x => x.id == id);

            if (type == null)
            {
                return Json(new { success = false });
            }

            _db.Coupons.Remove(type);

            try
            {
                _db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"db error: {ex.Message}"
                });
            }
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
                return Json(new { success = false });
            }

            var oldp = _db.Prices.SingleOrDefault(x => x.type == p.type);

            if (oldp == null)
            {
                return Json(new { success = false, message = "old price not found" });
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
                return Json(new { success = false, message = $"db error: {ex.Message}" });
            }

            return Json(new { success = true });
        }

        public IActionResult Late() => Json(_db.Dates.SingleOrDefault(x => x.name == "late"));

        [HttpPost]
        public IActionResult Late([FromBody] DateTime d)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false });
            }

            var old = _db.Dates.SingleOrDefault(x => x.name == "late");

            if (old == null)
            {
                return Json(new { success = false, message = $"Late date not found, application error" });
            }

            old.value = d;

            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"db error: {ex.Message}" });
            }

            return Json(new { success = true });
        }
    }
}