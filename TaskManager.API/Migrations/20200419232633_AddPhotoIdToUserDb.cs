using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskManager.API.Migrations
{
    public partial class AddPhotoIdToUserDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "AspNetUsers");
        }
    }
}
