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
    }
}
