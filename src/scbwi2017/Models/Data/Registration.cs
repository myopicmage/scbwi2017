using scbwi2017.Models.RegistrationViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scbwi2017.Models.Data
{
    public class Registration : Common
    {
        public ApplicationUser user { get; set; }
        public RegistrationType type { get; set; }
        public bool takingbus { get; set; }
        public Meal meal { get; set; }
        public virtual ICollection<Extra> Extras { get; set; }
        public Workshop first { get; set; }
        public Workshop second { get; set; }
        public int manuscript { get; set; }
        public int portfolio { get; set; }
        public Coupon coupon { get; set; }
        
        public DateTime paid { get; set; }
        public DateTime cleared { get; set; }

        public decimal subtotal { get; set; }
        public decimal total { get; set; }

        public string paypalid { get; set; }

        public void SetPayPalId()
        {
            paypalid = $"{user.firstname}{user.lastname}{created.Ticks}";
        }

        public Registration() { }

        public Registration(RegistrationViewModel r)
        {
            user = new ApplicationUser();

            user.firstname = r.user.first;
            user.lastname = r.user.last;
            user.phone = r.user.phone;
            user.postalcode = r.user.zip;
            user.address1 = r.user.address1;
            user.address2 = r.user.address2;
            user.city = r.user.city;
            user.Email = r.user.email;
            user.UserName = r.user.email;
        }
    }
}
