using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scbwi2017.Models.Data
{
    public class Workshop : Common
    {
        public WorkshopTime time { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string presenters { get; set; }
        public int maxattendees { get; set; }
    }

    public enum WorkshopTime
    {
        First,
        Second
    }
}
