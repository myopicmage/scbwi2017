using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace scbwi2017.Data.Migrations
{
    public partial class regarchive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegArchive",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Email = table.Column<string>(nullable: true),
                    address1 = table.Column<string>(nullable: true),
                    address2 = table.Column<string>(nullable: true),
                    city = table.Column<string>(nullable: true),
                    cleared = table.Column<string>(nullable: true),
                    comprehensive = table.Column<string>(nullable: true),
                    coupon = table.Column<string>(nullable: true),
                    created = table.Column<string>(nullable: true),
                    first_workshop = table.Column<string>(nullable: true),
                    firstname = table.Column<string>(nullable: true),
                    lastname = table.Column<string>(nullable: true),
                    manuscript = table.Column<int>(nullable: false),
                    meal = table.Column<string>(nullable: true),
                    paid = table.Column<string>(nullable: true),
                    phone = table.Column<string>(nullable: true),
                    portfolio = table.Column<int>(nullable: false),
                    postalcode = table.Column<string>(nullable: true),
                    registrationtype = table.Column<string>(nullable: true),
                    satdinner = table.Column<int>(nullable: false),
                    second_workshop = table.Column<string>(nullable: true),
                    state = table.Column<string>(nullable: true),
                    subtotal = table.Column<decimal>(nullable: false),
                    takingbus = table.Column<string>(nullable: true),
                    total = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegArchive", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Extras_Registrations_Registrationid",
                table: "Extras");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Extras_comprehensiveid",
                table: "Registrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Coupons_couponid",
                table: "Registrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Workshops_firstid",
                table: "Registrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Meals_mealid",
                table: "Registrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Workshops_secondid",
                table: "Registrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Types_typeid",
                table: "Registrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_AspNetUsers_userId",
                table: "Registrations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Registrations",
                table: "Registrations");

            migrationBuilder.DropTable(
                name: "RegArchive");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Registration",
                table: "Registrations",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Extras_Registration_Registrationid",
                table: "Extras",
                column: "Registrationid",
                principalTable: "Registrations",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Registration_Extras_comprehensiveid",
                table: "Registrations",
                column: "comprehensiveid",
                principalTable: "Extras",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Registration_Coupons_couponid",
                table: "Registrations",
                column: "couponid",
                principalTable: "Coupons",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Registration_Workshops_firstid",
                table: "Registrations",
                column: "firstid",
                principalTable: "Workshops",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Registration_Meals_mealid",
                table: "Registrations",
                column: "mealid",
                principalTable: "Meals",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Registration_Workshops_secondid",
                table: "Registrations",
                column: "secondid",
                principalTable: "Workshops",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Registration_Types_typeid",
                table: "Registrations",
                column: "typeid",
                principalTable: "Types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Registration_AspNetUsers_userId",
                table: "Registrations",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "IX_Registrations_userId",
                table: "Registrations",
                newName: "IX_Registration_userId");

            migrationBuilder.RenameIndex(
                name: "IX_Registrations_typeid",
                table: "Registrations",
                newName: "IX_Registration_typeid");

            migrationBuilder.RenameIndex(
                name: "IX_Registrations_secondid",
                table: "Registrations",
                newName: "IX_Registration_secondid");

            migrationBuilder.RenameIndex(
                name: "IX_Registrations_mealid",
                table: "Registrations",
                newName: "IX_Registration_mealid");

            migrationBuilder.RenameIndex(
                name: "IX_Registrations_firstid",
                table: "Registrations",
                newName: "IX_Registration_firstid");

            migrationBuilder.RenameIndex(
                name: "IX_Registrations_couponid",
                table: "Registrations",
                newName: "IX_Registration_couponid");

            migrationBuilder.RenameIndex(
                name: "IX_Registrations_comprehensiveid",
                table: "Registrations",
                newName: "IX_Registration_comprehensiveid");

            migrationBuilder.RenameTable(
                name: "Registrations",
                newName: "Registration");
        }
    }
}
