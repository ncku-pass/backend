using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addResumeAndTopic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "resume",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    create_time = table.Column<DateTime>(nullable: false),
                    update_time = table.Column<DateTime>(nullable: true),
                    name = table.Column<string>(maxLength: 30, nullable: false),
                    user_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_resume", x => x.id);
                    table.ForeignKey(
                        name: "fk_resume_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "topic",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    create_time = table.Column<DateTime>(nullable: false),
                    update_time = table.Column<DateTime>(nullable: true),
                    name = table.Column<string>(maxLength: 30, nullable: false),
                    resume_id = table.Column<int>(nullable: false),
                    user_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_topic", x => x.id);
                    table.ForeignKey(
                        name: "fk_topic_resume_resume_id",
                        column: x => x.resume_id,
                        principalTable: "resume",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_topic_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "topic_experience",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    create_time = table.Column<DateTime>(nullable: false),
                    update_time = table.Column<DateTime>(nullable: true),
                    topic_id = table.Column<int>(nullable: false),
                    experience_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_topic_experience", x => x.id);
                    table.ForeignKey(
                        name: "fk_topic_experience_experiences_experience_id",
                        column: x => x.experience_id,
                        principalTable: "experiences",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_topic_experience_topic_topic_id",
                        column: x => x.topic_id,
                        principalTable: "topic",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_resume_user_id",
                table: "resume",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_topic_resume_id",
                table: "topic",
                column: "resume_id");

            migrationBuilder.CreateIndex(
                name: "ix_topic_user_id",
                table: "topic",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_topic_experience_experience_id",
                table: "topic_experience",
                column: "experience_id");

            migrationBuilder.CreateIndex(
                name: "ix_topic_experience_topic_id",
                table: "topic_experience",
                column: "topic_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "topic_experience");

            migrationBuilder.DropTable(
                name: "topic");

            migrationBuilder.DropTable(
                name: "resume");
        }
    }
}
