using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class experienceStartEndTime_TypeUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "link",
                table: "experiences",
                type: "Text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "feedback",
                table: "experiences",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(150) CHARACTER SET utf8mb4",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "experiences",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100) CHARACTER SET utf8mb4",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<DateTime>(
                name: "date_end",
                table: "experiences",
                type: "Date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "date_start",
                table: "experiences",
                type: "Date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date_end",
                table: "experiences");

            migrationBuilder.DropColumn(
                name: "date_start",
                table: "experiences");

            migrationBuilder.AlterColumn<string>(
                name: "link",
                table: "experiences",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "Text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "feedback",
                table: "experiences",
                type: "varchar(150) CHARACTER SET utf8mb4",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "experiences",
                type: "varchar(100) CHARACTER SET utf8mb4",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 250);
        }
    }
}
