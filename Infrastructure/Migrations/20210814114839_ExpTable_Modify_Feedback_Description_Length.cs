using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class ExpTable_Modify_Feedback_Description_Length : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "feedback",
                table: "experiences",
                maxLength: 1050,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250) CHARACTER SET utf8mb4",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "experiences",
                maxLength: 550,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250) CHARACTER SET utf8mb4",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "categories",
                table: "experiences",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50) CHARACTER SET utf8mb4",
                oldMaxLength: 50,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "feedback",
                table: "experiences",
                type: "varchar(250) CHARACTER SET utf8mb4",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1050,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "experiences",
                type: "varchar(250) CHARACTER SET utf8mb4",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 550,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "categories",
                table: "experiences",
                type: "varchar(50) CHARACTER SET utf8mb4",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
