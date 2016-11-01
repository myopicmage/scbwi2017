using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scbwi2017.Models.Data
{
    public class Coupon : Common
    {
        public string text { get; set; }
        public string value { get; set; }
        public CouponType type { get; set; }
        public int validuses { get; set; }

        public virtual ICollection<Registration> users { get; set; }
    }

    public enum CouponType
    {
        PercentOff,
        Reduction,
        TotalCost
    }
}
