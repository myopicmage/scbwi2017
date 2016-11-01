using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scbwi2017.Models.Data
{
    public class RegistrationType : Common
    {
        public bool ismember { get; set; }
        public bool saturday { get; set; }
        public bool sunday { get; set; }
        public bool friday { get; set; }
        public bool allowsworkshops { get; set; }
        public decimal earlyprice { get; set; }
        public decimal lateprice { get; set; }
        public string description { get; set; }
        public string title { get; set; }
        public string presenters { get; set; }
    }
}
