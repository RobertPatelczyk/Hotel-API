using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelReview.Migrations
{
    public partial class aaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Hotels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_CreatedById",
                table: "Hotels",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_Users_CreatedById",
                table: "Hotels",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_Users_CreatedById",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_CreatedById",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Hotels");
        }
    }
}
