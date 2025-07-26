using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountantIdToBilling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accountants_AspNetUsers_UserId",
                table: "Accountants");

            migrationBuilder.DropForeignKey(
                name: "FK_Accountants_Staff_Managements_Staff_ManagementId",
                table: "Accountants");

            migrationBuilder.DropForeignKey(
                name: "FK_Staff_Managements_Departments_DepartmentId",
                table: "Staff_Managements");

            migrationBuilder.DropIndex(
                name: "IX_Accountants_Staff_ManagementId",
                table: "Accountants");

            migrationBuilder.DropIndex(
                name: "IX_Accountants_UserId",
                table: "Accountants");

            migrationBuilder.DropColumn(
                name: "Staff_ManagementId",
                table: "Accountants");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Accountants");

            migrationBuilder.AlterColumn<int>(
                name: "Salary",
                table: "Staff_Managements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Staff_Managements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateHired",
                table: "Staff_Managements",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_Managements_Departments_DepartmentId",
                table: "Staff_Managements",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Accountants_accountantId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Staff_Managements_Departments_DepartmentId",
                table: "Staff_Managements");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_accountantId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "accountantId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "Salary",
                table: "Staff_Managements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Staff_Managements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateHired",
                table: "Staff_Managements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Staff_ManagementId",
                table: "Accountants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Accountants",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accountants_Staff_ManagementId",
                table: "Accountants",
                column: "Staff_ManagementId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Accountants_Staff_Managements_Staff_ManagementId",
                table: "Accountants",
                column: "Staff_ManagementId",
                principalTable: "Staff_Managements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_Managements_Departments_DepartmentId",
                table: "Staff_Managements",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
