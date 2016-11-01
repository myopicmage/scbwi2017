using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace scbwi2017.Data.Migrations
{
    public partial class imdumb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "Extras",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "price",
                table: "Extras");
        }
    }
}
