using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskManager.API.Migrations
{
    public partial class AddOwnerToTaskDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Owner",
                table: "PTasks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "PTasks");
        }
    }
}
