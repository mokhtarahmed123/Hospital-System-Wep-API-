using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIDtoAccountant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Accountants_accountantId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_accountantId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "accountantId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Accountants",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accountants_UserId",
                table: "Accountants",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Accountants_AspNetUsers_UserId",
                table: "Accountants",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accountants_AspNetUsers_UserId",
                table: "Accountants");

            migrationBuilder.DropIndex(
                name: "IX_Accountants_UserId",
                table: "Accountants");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Accountants");

            migrationBuilder.AddColumn<int>(
                name: "accountantId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_accountantId",
                table: "AspNetUsers",
                column: "accountantId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Accountants_accountantId",
                table: "AspNetUsers",
                column: "accountantId",
                principalTable: "Accountants",
                principalColumn: "Id");
        }
    }
}
