using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class renameCardTable_cardType_to_Type : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "card_type",
                table: "cards");

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "cards",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "cards");

            migrationBuilder.AddColumn<int>(
                name: "card_type",
                table: "cards",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}