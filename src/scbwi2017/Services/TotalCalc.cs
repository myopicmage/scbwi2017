using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using scbwi2017.Controllers;
using scbwi2017.Data;
using scbwi2017.Models.Data;
using scbwi2017.Models.RegistrationViewModels;

namespace scbwi2017.Services
{
    public class TotalCalc
    {
        public static Totals CalcTotal(RegistrationViewModel r, ApplicationDbContext _db, ILogger _logger)
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
                case CouponType.FreeBase:
                    total = subtotal - (DateTime.Now > late.value ? reg.lateprice : reg.earlyprice);
                    message = "This coupon is good for a free base conference!";
                    break;
                case CouponType.HalfOffBase:
                    total = subtotal - ((DateTime.Now > late.value ? reg.lateprice : reg.earlyprice) / 2);
                    message = "This coupon is good for 50% off your base conference!";
                    break;
                case CouponType.FreeComprehensive:
                    if (r.comprehensive > 0)
                    {
                        var c = _db.Extras.SingleOrDefault(x => x.id == r.comprehensive);
                        total = subtotal - c.price;
                        message = "This coupon is good for a free comprehensive!";
                    }
                    break;
                case CouponType.FreeCritique:
                    if (r.manuscripts > 0 || r.portfolios > 0)
                    {
                        total = subtotal - m_price.value;
                        message = "This coupon is good for a free critique (manuscript or portfolio)";
                    }
                    break;
                case CouponType.FreeConferenceAndComprehensive:
                    total = subtotal - (DateTime.Now > late.value ? reg.lateprice : reg.earlyprice);
                    if (r.comprehensive > 0)
                    {
                        var c = _db.Extras.SingleOrDefault(x => x.id == r.comprehensive);
                        total -= subtotal - c.price;
                    }
                    message = "This coupon is good for a free conference AND free comprehensive!";
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
