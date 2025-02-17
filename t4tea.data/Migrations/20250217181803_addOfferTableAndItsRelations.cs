using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace t4tea.data.Migrations
{
    public partial class addOfferTableAndItsRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OfferId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Offer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offer", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_OfferId",
                table: "Product",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Offer_OfferId",
                table: "Product",
                column: "OfferId",
                principalTable: "Offer",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Offer_OfferId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "Offer");

            migrationBuilder.DropIndex(
                name: "IX_Product_OfferId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "Product");
        }
    }
}
