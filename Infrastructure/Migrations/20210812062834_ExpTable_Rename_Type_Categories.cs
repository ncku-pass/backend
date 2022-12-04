using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class ExpTable_Rename_Type_Categories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "experience_type",
                table: "experiences",
                newName: "type");
            migrationBuilder.RenameColumn(
                name: "category",
                table: "experiences",
                newName: "categories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "type",
                table: "experiences",
                newName: "experience_type");
            migrationBuilder.RenameColumn(
                name: "categories",
                table: "experiences",
                newName: "category");
        }
    }
}