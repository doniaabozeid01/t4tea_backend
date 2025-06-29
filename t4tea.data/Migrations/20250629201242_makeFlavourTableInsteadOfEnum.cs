using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace t4tea.data.Migrations
{
    public partial class makeFlavourTableInsteadOfEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Flavour",
                table: "Product",
                newName: "flavourId");

            migrationBuilder.CreateTable(
                name: "Flavours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flavours", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_flavourId",
                table: "Product",
                column: "flavourId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Flavours_flavourId",
                table: "Product",
                column: "flavourId",
                principalTable: "Flavours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Flavours_flavourId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "Flavours");

            migrationBuilder.DropIndex(
                name: "IX_Product_flavourId",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "flavourId",
                table: "Product",
                newName: "Flavour");
        }
    }
}
