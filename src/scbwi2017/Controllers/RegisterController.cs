using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using scbwi2017.Data;
using scbwi2017.Models.Data;
using scbwi2017.Models.RegistrationViewModels;
using Braintree;
using Microsoft.Extensions.Options;
using scbwi2017.Services;

namespace scbwi2017.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly BraintreeGateway _gateway;
        private readonly ILogger _logger;
        private readonly IEmailSender _email;

        public RegisterController(ApplicationDbContext ctx, ILoggerFactory factory, IOptions<Secrets> sec, IEmailSender esvc)
        {
            _db = ctx;
            _logger = factory.CreateLogger("All");
            _gateway = new BraintreeGateway(sec.Value.paypaltoken);
            _email = esvc;
        }

        public IActionResult Index() => View();

        public IActionResult Update(Guid id)
        {
            ViewBag["id"] = id;

            return View();
        }

        public IActionResult GetToken()
        {
            try
            {
                var token = _gateway.ClientToken.generate();
                
                return Json(token);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed to generate token! {ex.Message}");
                throw;
            }
        }

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
            return Json(TotalCalc.CalcTotal(r, _db, _logger));
        }

        public async Task<IActionResult> Register([FromBody] RegistrationViewModel r)
        {
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

            var request = new TransactionRequest
            {
                Amount = totals.total,
                MerchantAccountId = "USD",
                PaymentMethodNonce = r.nonce,
                Options = new TransactionOptionsRequest
                {
                    PayPal = new TransactionOptionsPayPalRequest
                    {
                        CustomField = reg.paypalid,
                        Description = "SCBWI Florida January 2017 Conference",
                    },
                    SubmitForSettlement = true
                }
            };

            var result = _gateway.Transaction.Sale(request);

            if (result.IsSuccess())
            {
                _logger.LogInformation($"Transaction ID {result.Target.Id} has passed");

                reg.paid = result.Target.CreatedAt ?? DateTime.Now;
                reg.cleared = result.Target.CreatedAt ?? DateTime.Now;

                _db.Registrations.Add(reg);

                try
                {
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("Registration", ex);

                    return Json(new
                    {
                        success = false,
                        error = "Something went wrong saving your payment. Please do not submit again.",
                        submitagain = false
                    });
                }

                try
                {
                    var emailResp =
                        await
                            _email.SendEmailAsync(reg.user.Email, "Successful Registration",
                                reg.GenEmail(),
                                $"{reg.user.firstname} {reg.user.lastname}");

                    if (!emailResp.IsSuccessStatusCode)
                    {
                        _logger.LogWarning($"Failed to send confirmation to {reg.user.Email}. Reason: {emailResp.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Failed to send registration confirmation due to {ex.Message}");
                }

                return Json(new
                {
                    success = true
                });
            }

            _logger.LogInformation($"Transaction ID {result.Target.Id} has failed");

            return Json(new
            {
                success = false,
                error = string.Join("\n", result.Errors.All()),
                submitagain = true
            });
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

    }

}