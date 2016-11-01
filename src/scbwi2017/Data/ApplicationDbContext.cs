using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using scbwi2017.Models;
using scbwi2017.Models.Data;

namespace scbwi2017.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public virtual DbSet<Coupon> Coupons { get; set; }
        public virtual DbSet<Date> Dates { get; set; }
        public virtual DbSet<Extra> Extras { get; set; }
        public virtual DbSet<Meal> Meals { get; set; }
        public virtual DbSet<Price> Prices { get; set; }
        public virtual DbSet<Registration> Registrations { get; set; }
        public virtual DbSet<RegistrationType> Types { get; set; }
        public virtual DbSet<Workshop> Workshops { get; set; }
    }
}
