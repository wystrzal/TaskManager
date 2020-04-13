using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskManager.API.Migrations
{
    public partial class AddTaskToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "PTasks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "PTasks",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeToEnd",
                table: "PTasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "PTasks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "PTasks");

            migrationBuilder.DropColumn(
                name: "TimeToEnd",
                table: "PTasks");
        }
    }
}
