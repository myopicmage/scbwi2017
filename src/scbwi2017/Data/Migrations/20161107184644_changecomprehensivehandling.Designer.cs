using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using scbwi2017.Data;

namespace scbwi2017.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20161107184644_changecomprehensivehandling")]
    partial class changecomprehensivehandling
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("scbwi2017.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("address1");

                    b.Property<string>("address2");

                    b.Property<string>("city");

                    b.Property<string>("firstname");

                    b.Property<string>("lastname");

                    b.Property<string>("phone");

                    b.Property<string>("postalcode");

                    b.Property<string>("state");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("scbwi2017.Models.Data.Coupon", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("created");

                    b.Property<string>("createdby");

                    b.Property<DateTime>("modified");

                    b.Property<string>("modifiedby");

                    b.Property<string>("text");

                    b.Property<int>("type");

                    b.Property<int>("validuses");

                    b.Property<string>("value");

                    b.HasKey("id");

                    b.ToTable("Coupons");
                });

            modelBuilder.Entity("scbwi2017.Models.Data.Date", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("created");

                    b.Property<string>("createdby");

                    b.Property<string>("description");

                    b.Property<DateTime>("modified");

                    b.Property<string>("modifiedby");

                    b.Property<string>("name");

                    b.Property<DateTime>("value");

                    b.HasKey("id");

                    b.ToTable("Dates");
                });

            modelBuilder.Entity("scbwi2017.Models.Data.Extra", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Registrationid");

                    b.Property<int>("comprehensive");

                    b.Property<DateTime>("created");

                    b.Property<string>("createdby");

                    b.Property<string>("description");

                    b.Property<decimal>("extracost");

                    b.Property<int>("maxattendees");

                    b.Property<DateTime>("modified");

                    b.Property<string>("modifiedby");

                    b.Property<decimal>("price");

                    b.Property<bool>("requiresattendance");

                    b.Property<bool>("requiresmember");

                    b.Property<string>("title");

                    b.HasKey("id");

                    b.HasIndex("Registrationid");

                    b.ToTable("Extras");
                });

            modelBuilder.Entity("scbwi2017.Models.Data.Meal", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("created");

                    b.Property<string>("createdby");

                    b.Property<string>("description");

                    b.Property<int>("mealtype");

                    b.Property<DateTime>("modified");

                    b.Property<string>("modifiedby");

                    b.Property<string>("title");

                    b.HasKey("id");

                    b.ToTable("Meals");
                });

            modelBuilder.Entity("scbwi2017.Models.Data.Price", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("created");

                    b.Property<string>("createdby");

                    b.Property<DateTime>("modified");

                    b.Property<string>("modifiedby");

                    b.Property<string>("type");

                    b.Property<decimal>("value");

                    b.HasKey("id");

                    b.ToTable("Prices");
                });

            modelBuilder.Entity("scbwi2017.Models.Data.Registration", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("cleared");

                    b.Property<int?>("comprehensiveid");

                    b.Property<int?>("couponid");

                    b.Property<DateTime>("created");

                    b.Property<string>("createdby");

                    b.Property<int?>("firstid");

                    b.Property<int>("manuscript");

                    b.Property<int?>("mealid");

                    b.Property<DateTime>("modified");

                    b.Property<string>("modifiedby");

                    b.Property<DateTime>("paid");

                    b.Property<string>("paypalid");

                    b.Property<int>("portfolio");

                    b.Property<int>("satdinner");

                    b.Property<int?>("secondid");

                    b.Property<decimal>("subtotal");

                    b.Property<bool>("takingbus");

                    b.Property<decimal>("total");

                    b.Property<int?>("typeid");

                    b.Property<string>("userId");

                    b.HasKey("id");

                    b.HasIndex("comprehensiveid");

                    b.HasIndex("couponid");

                    b.HasIndex("firstid");

                    b.HasIndex("mealid");

                    b.HasIndex("secondid");

                    b.HasIndex("typeid");

                    b.HasIndex("userId");

                    b.ToTable("Registrations");
                });

            modelBuilder.Entity("scbwi2017.Models.Data.RegistrationType", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("allowsworkshops");

                    b.Property<DateTime>("created");

                    b.Property<string>("createdby");

                    b.Property<string>("description");

                    b.Property<decimal>("earlyprice");

                    b.Property<bool>("friday");

                    b.Property<bool>("ismember");

                    b.Property<decimal>("lateprice");

                    b.Property<DateTime>("modified");

                    b.Property<string>("modifiedby");

                    b.Property<string>("presenters");

                    b.Property<bool>("saturday");

                    b.Property<bool>("sunday");

                    b.Property<string>("title");

                    b.HasKey("id");

                    b.ToTable("Types");
                });

            modelBuilder.Entity("scbwi2017.Models.Data.Workshop", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("created");

                    b.Property<string>("createdby");

                    b.Property<string>("description");

                    b.Property<int>("maxattendees");

                    b.Property<DateTime>("modified");

                    b.Property<string>("modifiedby");

                    b.Property<string>("presenters");

                    b.Property<int>("time");

                    b.Property<string>("title");

                    b.HasKey("id");

                    b.ToTable("Workshops");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("scbwi2017.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("scbwi2017.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("scbwi2017.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("scbwi2017.Models.Data.Extra", b =>
                {
                    b.HasOne("scbwi2017.Models.Data.Registration")
                        .WithMany("Extras")
                        .HasForeignKey("Registrationid");
                });

            modelBuilder.Entity("scbwi2017.Models.Data.Registration", b =>
                {
                    b.HasOne("scbwi2017.Models.Data.Extra", "comprehensive")
                        .WithMany()
                        .HasForeignKey("comprehensiveid");

                    b.HasOne("scbwi2017.Models.Data.Coupon", "coupon")
                        .WithMany("users")
                        .HasForeignKey("couponid");

                    b.HasOne("scbwi2017.Models.Data.Workshop", "first")
                        .WithMany()
                        .HasForeignKey("firstid");

                    b.HasOne("scbwi2017.Models.Data.Meal", "meal")
                        .WithMany()
                        .HasForeignKey("mealid");

                    b.HasOne("scbwi2017.Models.Data.Workshop", "second")
                        .WithMany()
                        .HasForeignKey("secondid");

                    b.HasOne("scbwi2017.Models.Data.RegistrationType", "type")
                        .WithMany()
                        .HasForeignKey("typeid");

                    b.HasOne("scbwi2017.Models.ApplicationUser", "user")
                        .WithMany()
                        .HasForeignKey("userId");
                });
        }
    }
}
