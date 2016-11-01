using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace scbwi2017.Data.Migrations
{
    public partial class infoadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    created = table.Column<DateTime>(nullable: false),
                    createdby = table.Column<string>(nullable: true),
                    modified = table.Column<DateTime>(nullable: false),
                    modifiedby = table.Column<string>(nullable: true),
                    text = table.Column<string>(nullable: true),
                    type = table.Column<int>(nullable: false),
                    validuses = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Dates",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    created = table.Column<DateTime>(nullable: false),
                    createdby = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    modified = table.Column<DateTime>(nullable: false),
                    modifiedby = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    value = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Meals",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    created = table.Column<DateTime>(nullable: false),
                    createdby = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    mealtype = table.Column<int>(nullable: false),
                    modified = table.Column<DateTime>(nullable: false),
                    modifiedby = table.Column<string>(nullable: true),
                    title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    allowsworkshops = table.Column<bool>(nullable: false),
                    created = table.Column<DateTime>(nullable: false),
                    createdby = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    earlyprice = table.Column<decimal>(nullable: false),
                    friday = table.Column<bool>(nullable: false),
                    ismember = table.Column<bool>(nullable: false),
                    lateprice = table.Column<decimal>(nullable: false),
                    modified = table.Column<DateTime>(nullable: false),
                    modifiedby = table.Column<string>(nullable: true),
                    presenters = table.Column<string>(nullable: true),
                    saturday = table.Column<bool>(nullable: false),
                    sunday = table.Column<bool>(nullable: false),
                    title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Workshop",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    created = table.Column<DateTime>(nullable: false),
                    createdby = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    maxattendees = table.Column<int>(nullable: false),
                    modified = table.Column<DateTime>(nullable: false),
                    modifiedby = table.Column<string>(nullable: true),
                    presenters = table.Column<string>(nullable: true),
                    time = table.Column<int>(nullable: false),
                    title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workshop", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    cleared = table.Column<DateTime>(nullable: false),
                    couponid = table.Column<int>(nullable: true),
                    created = table.Column<DateTime>(nullable: false),
                    createdby = table.Column<string>(nullable: true),
                    firstid = table.Column<int>(nullable: true),
                    mealid = table.Column<int>(nullable: true),
                    modified = table.Column<DateTime>(nullable: false),
                    modifiedby = table.Column<string>(nullable: true),
                    paid = table.Column<DateTime>(nullable: false),
                    secondid = table.Column<int>(nullable: true),
                    subtotal = table.Column<decimal>(nullable: false),
                    takingbus = table.Column<bool>(nullable: false),
                    total = table.Column<decimal>(nullable: false),
                    typeid = table.Column<int>(nullable: true),
                    userId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.id);
                    table.ForeignKey(
                        name: "FK_Registrations_Coupons_couponid",
                        column: x => x.couponid,
                        principalTable: "Coupons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registrations_Workshop_firstid",
                        column: x => x.firstid,
                        principalTable: "Workshop",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registrations_Meals_mealid",
                        column: x => x.mealid,
                        principalTable: "Meals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registrations_Workshop_secondid",
                        column: x => x.secondid,
                        principalTable: "Workshop",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registrations_Types_typeid",
                        column: x => x.typeid,
                        principalTable: "Types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registrations_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Extras",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Registrationid = table.Column<int>(nullable: true),
                    comprehensive = table.Column<int>(nullable: false),
                    created = table.Column<DateTime>(nullable: false),
                    createdby = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    extracost = table.Column<decimal>(nullable: false),
                    maxattendees = table.Column<int>(nullable: false),
                    modified = table.Column<DateTime>(nullable: false),
                    modifiedby = table.Column<string>(nullable: true),
                    requiresattendance = table.Column<bool>(nullable: false),
                    requiresmember = table.Column<bool>(nullable: false),
                    title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Extras", x => x.id);
                    table.ForeignKey(
                        name: "FK_Extras_Registrations_Registrationid",
                        column: x => x.Registrationid,
                        principalTable: "Registrations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Extras_Registrationid",
                table: "Extras",
                column: "Registrationid");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_couponid",
                table: "Registrations",
                column: "couponid");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_firstid",
                table: "Registrations",
                column: "firstid");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_mealid",
                table: "Registrations",
                column: "mealid");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_secondid",
                table: "Registrations",
                column: "secondid");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_typeid",
                table: "Registrations",
                column: "typeid");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_userId",
                table: "Registrations",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dates");

            migrationBuilder.DropTable(
                name: "Extras");

            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.DropTable(
                name: "Workshop");

            migrationBuilder.DropTable(
                name: "Meals");

            migrationBuilder.DropTable(
                name: "Types");
        }
    }
}
