using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scbwi2017.Models.Data
{
    public abstract class Common
    {
        public int id { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string createdby { get; set; }
        public string modifiedby { get; set; }
    }
}
