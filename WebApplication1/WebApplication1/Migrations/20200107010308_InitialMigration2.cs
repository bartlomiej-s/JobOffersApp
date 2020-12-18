using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class InitialMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_JobOfers_JobOfferId",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_JobOfferId",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "JobOfferId",
                table: "JobApplications");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "JobApplications",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "JobApplications",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "JobApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "JobApplications",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_OfferId",
                table: "JobApplications",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_JobOfers_OfferId",
                table: "JobApplications",
                column: "OfferId",
                principalTable: "JobOfers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_JobOfers_OfferId",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_OfferId",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "JobApplications");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "JobApplications",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "JobApplications",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "JobOfferId",
                table: "JobApplications",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_JobOfferId",
                table: "JobApplications",
                column: "JobOfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_JobOfers_JobOfferId",
                table: "JobApplications",
                column: "JobOfferId",
                principalTable: "JobOfers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
