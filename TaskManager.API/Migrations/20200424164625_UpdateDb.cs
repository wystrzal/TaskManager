using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskManager.API.Migrations
{
    public partial class UpdateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PTasks_Projects_ProjectId",
                table: "PTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_PTasks_Projects_ProjectId",
                table: "PTasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PTasks_Projects_ProjectId",
                table: "PTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_PTasks_Projects_ProjectId",
                table: "PTasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
