using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DigitalProductAPI.Migrations
{
    public partial class SecuritiesContextModelSecond : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AuthorizeDate",
                table: "TreasuryBills",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "TreasuryBills",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "AuthorizeDate",
                table: "Bonds",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Bonds",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorizeDate",
                table: "TreasuryBills");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "TreasuryBills");

            migrationBuilder.DropColumn(
                name: "AuthorizeDate",
                table: "Bonds");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Bonds");
        }
    }
}
