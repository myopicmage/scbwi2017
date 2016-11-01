using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using scbwi2017.Models.Data;

namespace scbwi2017.Data.Migrations
{
    public partial class workshops : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "manuscript",
                table: "Registrations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "paypalid",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "portfolio",
                table: "Registrations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.RenameTable(
                name: "Workshop",
                newName: "Workshops");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Workshops_firstid",
                table: "Registrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Workshops_secondid",
                table: "Registrations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Workshops",
                table: "Workshops");

            migrationBuilder.DropColumn(
                name: "manuscript",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "paypalid",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "portfolio",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "comprehensive",
                table: "Extras");

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "Extras",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Workshop",
                table: "Workshops",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_Workshop_firstid",
                table: "Registrations",
                column: "firstid",
                principalTable: "Workshops",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_Workshop_secondid",
                table: "Registrations",
                column: "secondid",
                principalTable: "Workshops",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameTable(
                name: "Workshops",
                newName: "Workshop");
        }
    }
}
