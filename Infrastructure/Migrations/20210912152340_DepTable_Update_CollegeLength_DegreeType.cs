using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class DepTable_Update_CollegeLength_DegreeType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "degree",
                table: "departments",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "college",
                table: "departments",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1) CHARACTER SET utf8mb4",
                oldMaxLength: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "degree",
                table: "departments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "college",
                table: "departments",
                type: "varchar(1) CHARACTER SET utf8mb4",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20);
        }
    }
}