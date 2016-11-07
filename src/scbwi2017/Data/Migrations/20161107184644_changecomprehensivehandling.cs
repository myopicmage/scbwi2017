using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace scbwi2017.Data.Migrations
{
    public partial class changecomprehensivehandling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "comprehensiveid",
                table: "Registrations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_comprehensiveid",
                table: "Registrations",
                column: "comprehensiveid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Extras_comprehensiveid",
                table: "Registrations");

            migrationBuilder.DropIndex(
                name: "IX_Registrations_comprehensiveid",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "comprehensiveid",
                table: "Registrations");
        }
    }
}
