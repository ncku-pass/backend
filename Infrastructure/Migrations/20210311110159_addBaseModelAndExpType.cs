using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addBaseModelAndExpType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "create_time",
                table: "tags",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "update_time",
                table: "tags",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "create_time",
                table: "tag_experiences",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "update_time",
                table: "tag_experiences",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "experience_type",
                table: "experiences",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "create_time",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "update_time",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "create_time",
                table: "tag_experiences");

            migrationBuilder.DropColumn(
                name: "update_time",
                table: "tag_experiences");

            migrationBuilder.DropColumn(
                name: "experience_type",
                table: "experiences");
        }
    }
}
