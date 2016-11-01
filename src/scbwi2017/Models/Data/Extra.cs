using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scbwi2017.Models.Data
{
    public class Extra : Common
    {
        public bool requiresmember { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public bool requiresattendance { get; set; }
        public ExtraType comprehensive { get; set; }
        public decimal extracost { get; set; }
        public int maxattendees { get; set; }
        public decimal price { get; set; }
    }

    public enum ExtraType
    {
        Comprehensive,
        SaturdayDinner
    }
}
