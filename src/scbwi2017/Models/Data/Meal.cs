using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scbwi2017.Models.Data
{
    public class Meal : Common
    {
        public string title { get; set; }
        public string description { get; set; }
        public MealType mealtype { get; set; }
    }

    public enum MealType
    {
        Lunch,
        Dinner
    }
}
