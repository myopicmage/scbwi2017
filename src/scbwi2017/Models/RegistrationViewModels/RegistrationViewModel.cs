using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scbwi2017.Models.RegistrationViewModels
{
    public class RegistrationViewModel
    {
        public int comprehensive { get; set; }
        public string coupon { get; set; }
        public int first { get; set; }
        public int manuscripts { get; set; }
        public int meal { get; set; }
        public int portfolios { get; set; }
        public int satdinner { get; set; }
        public int second { get; set; }
        public string takingbus { get; set; }
        public int type { get; set; }
        public UserViewModel user { get; set; }
    }

    public class UserViewModel
    { 
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public string first { get; set; }
        public string last { get; set; }
        public string phone { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
    }
}
