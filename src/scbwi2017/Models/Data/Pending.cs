using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scbwi2017.Models.Data
{
    public class Pending : Common
    {
        public virtual Registration r { get; set; }
        public decimal total { get; set; }
        public bool active { get; set; }
        public DateTime paid { get; set; }
        public bool comprehensive { get; set; }
        public string c_name { get; set; }
        public int satdinner { get; set; }
        public int manuscript { get; set; }
        public int portfolio { get; set; }
        public Guid displayid { get; set; }
        public string message { get; set; }
    }
}
