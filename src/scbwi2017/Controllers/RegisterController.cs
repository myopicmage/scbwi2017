using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using scbwi2017.Data;
using scbwi2017.Models.Data;
using scbwi2017.Models.RegistrationViewModels;

namespace scbwi2017.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger _logger;

        public RegisterController(ApplicationDbContext ctx, ILoggerFactory factory)
        {
            _db = ctx;
            _logger = factory.CreateLogger("All");
        }

        public IActionResult Index() => View();

        public IActionResult RegTypes([FromBody] bool member)
        {
            _logger.LogInformation($"Member: {member}");

            var data = _db.Types.Where(x => x.ismember == member).ToList();

            return Json(data);
        }

        public IActionResult Comprehensives()
        {
            var data = _db.Extras
                .Where(x => x.comprehensive == ExtraType.Comprehensive)
                .Where(NotFull)
                .ToList();

            return Json(data);
        }

        public IActionResult Workshops()
        {
            var workshops = _db.Workshops.Where(NotFull);
            var early = workshops.Where(x => x.time == WorkshopTime.First);
            var late = workshops.Where(x => x.time == WorkshopTime.Second);

            return Json(new { early, late });
        }

        public IActionResult Meals() => Json(_db.Meals.ToList());

        public IActionResult Price(string type)
        {
            var types = type.Split(',');

            var prices = _db.Prices
                .Where(x => types.Contains(x.type))
                .Select(x => new { x.type, x.value })
                .ToArray();

            return Json(prices);
        }

        public IActionResult Total([FromBody] RegistrationViewModel r)
        {
            return Json(CalcTotal(r));
        }

        private bool NotFull(Extra e)
        {
            var attendees = _db.Registrations
                .Count(x => x.Extras.Any(y => y.id == e.id));

            return attendees < e.maxattendees;
        }

        private bool NotFull(Workshop w)
        {
            var attendees = _db.Registrations
                .Count(x => x.first.id == w.id || x.second.id == w.id);

            return attendees < w.maxattendees;
        }

        private Totals CalcTotal(RegistrationViewModel r)
        {
            var subtotal = 0m;
            var total = 0m;
            var late = _db.Dates.SingleOrDefault(x => x.name == "late");
            var m_price = _db.Prices.SingleOrDefault(x => x.type == "manuscript");
            var p_price = _db.Prices.SingleOrDefault(x => x.type == "portfolio");
            var d_price = _db.Prices.SingleOrDefault(x => x.type == "satdinner");

            var reg = _db.Types.SingleOrDefault(x => x.id == r.type);

            // registration
            subtotal += DateTime.Now > late.value ? reg.lateprice : reg.earlyprice;

            // comprehensive
            if (r.comprehensive > 0)
            {
                var c = _db.Extras.SingleOrDefault(x => x.id == r.comprehensive);

                subtotal += reg.friday ? 0 : c.price;
            }

            // manuscript critiques
            subtotal += r.manuscripts * m_price.value;

            // portfolio
            subtotal += r.portfolios * p_price.value;

            // saturday dinner
            subtotal += r.satdinner * d_price.value;

            // no coupon
            if (string.IsNullOrEmpty(r.coupon))
            {
                total = subtotal;

                return new Totals(subtotal, total, false);
            }

            var c_text = r.coupon.ToLower();

            _logger.LogInformation($"Coupon attempt: {c_text}");

            var coupon = _db.Coupons.SingleOrDefault(x => x.text == c_text);

            // invalid coupon
            if (coupon == null)
            {
                total = subtotal;

                return new Totals(subtotal, total, false, "This coupon was invalid.");
            }

            var message = "";

            // valid coupon!
            switch (coupon.type)
            {
                case CouponType.TotalCost:
                    total = Convert.ToDecimal(coupon.value);
                    message = $"This coupon reduced your cost to {coupon.value}!";
                    break;
                case CouponType.PercentOff:
                    total = subtotal - (subtotal * Convert.ToDecimal(coupon.value) / 100);
                    message = $"This coupon is good for {coupon.value}% off!";
                    break;
                case CouponType.Reduction:
                    total = subtotal - Convert.ToDecimal(coupon.value);
                    message = $"This coupon is good for a ${coupon.value} discount!";
                    break;
                default:
                    total = subtotal;
                    break;
            }

            _logger.LogWarning($"Message: {message}");

            return new Totals(subtotal, total, true, message);
        }
    }

    public class Totals
    {
        public decimal subtotal { get; set; }
        public decimal total { get; set; }
        public bool validcoupon { get; set; }
        public string msg { get; set; }

        public Totals(decimal sub, decimal tot, bool valid, string msg = "")
        {
            subtotal = sub;
            total = tot;
            validcoupon = valid;
            this.msg = msg;
        }
    }
}