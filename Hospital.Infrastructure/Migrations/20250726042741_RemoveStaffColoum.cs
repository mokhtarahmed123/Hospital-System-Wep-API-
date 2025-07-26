using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStaffColoum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Staff_Managements_StaffMemberID",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "StaffMemberID",
                table: "Doctors",
                newName: "Staff_ManagementId");

            migrationBuilder.RenameIndex(
                name: "IX_Doctors_StaffMemberID",
                table: "Doctors",
                newName: "IX_Doctors_Staff_ManagementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Staff_Managements_Staff_ManagementId",
                table: "Doctors",
                column: "Staff_ManagementId",
                principalTable: "Staff_Managements",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Staff_Managements_Staff_ManagementId",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "Staff_ManagementId",
                table: "Doctors",
                newName: "StaffMemberID");

            migrationBuilder.RenameIndex(
                name: "IX_Doctors_Staff_ManagementId",
                table: "Doctors",
                newName: "IX_Doctors_StaffMemberID");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Staff_Managements_StaffMemberID",
                table: "Doctors",
                column: "StaffMemberID",
                principalTable: "Staff_Managements",
                principalColumn: "Id");
        }
    }
}
