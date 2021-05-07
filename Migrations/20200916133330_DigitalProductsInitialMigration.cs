using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DigitalProductAPI.Migrations
{
    public partial class DigitalProductsInitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bonds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SecurityID = table.Column<string>(nullable: false),
                    MaturityValue = table.Column<string>(nullable: false),
                    MaturityDate = table.Column<string>(nullable: false),
                    Rate = table.Column<decimal>(nullable: false),
                    BidPrice = table.Column<decimal>(nullable: false),
                    AvailableVolume = table.Column<decimal>(nullable: false),
                    OfferPrice = table.Column<decimal>(nullable: false),
                    Consideration = table.Column<decimal>(nullable: false),
                    FaceValue = table.Column<decimal>(nullable: false),
                    MakerId = table.Column<string>(nullable: false),
                    AuthorizerId = table.Column<string>(nullable: true),
                    InputDate = table.Column<DateTime>(nullable: false),
                    IsAuthorized = table.Column<bool>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bonds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TreasuryBills",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SecurityID = table.Column<string>(nullable: false),
                    MaturityValue = table.Column<string>(nullable: false),
                    MaturityDate = table.Column<string>(nullable: false),
                    Rate = table.Column<decimal>(nullable: false),
                    BidPrice = table.Column<decimal>(nullable: false),
                    AvailableVolume = table.Column<decimal>(nullable: false),
                    OfferPrice = table.Column<decimal>(nullable: false),
                    Consideration = table.Column<decimal>(nullable: false),
                    FaceValue = table.Column<decimal>(nullable: false),
                    MakerId = table.Column<string>(nullable: false),
                    AuthorizerId = table.Column<string>(nullable: true),
                    InputDate = table.Column<DateTime>(nullable: false),
                    IsAuthorized = table.Column<bool>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreasuryBills", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bonds");

            migrationBuilder.DropTable(
                name: "TreasuryBills");
        }
    }
}
