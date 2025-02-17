using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace t4tea.data.Migrations
{
    public partial class updateNameByDescriptionInBenifits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Benifit");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "Benifit",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "Benifit");

            migrationBuilder.AddColumn<int>(
                name: "Name",
                table: "Benifit",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
