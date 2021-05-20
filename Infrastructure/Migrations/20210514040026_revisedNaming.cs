using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class revisedNaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_experiences_user_user_id",
                table: "experiences");

            migrationBuilder.DropForeignKey(
                name: "fk_resume_user_user_id",
                table: "resume");

            migrationBuilder.DropForeignKey(
                name: "fk_tags_user_user_id",
                table: "tags");

            migrationBuilder.DropForeignKey(
                name: "fk_topic_resume_resume_id",
                table: "topic");

            migrationBuilder.DropForeignKey(
                name: "fk_topic_user_user_id",
                table: "topic");

            migrationBuilder.DropForeignKey(
                name: "fk_topic_experience_experiences_experience_id",
                table: "topic_experience");

            migrationBuilder.DropForeignKey(
                name: "fk_topic_experience_topic_topic_id",
                table: "topic_experience");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "pk_topic_experience",
                table: "topic_experience");

            migrationBuilder.DropPrimaryKey(
                name: "pk_topic",
                table: "topic");

            migrationBuilder.DropPrimaryKey(
                name: "pk_resume",
                table: "resume");

            migrationBuilder.RenameTable(
                name: "user",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "topic_experience",
                newName: "topic_experiences");

            migrationBuilder.RenameTable(
                name: "topic",
                newName: "topics");

            migrationBuilder.RenameTable(
                name: "resume",
                newName: "resumes");

            migrationBuilder.RenameIndex(
                name: "ix_topic_experience_topic_id",
                table: "topic_experiences",
                newName: "ix_topic_experiences_topic_id");

            migrationBuilder.RenameIndex(
                name: "ix_topic_experience_experience_id",
                table: "topic_experiences",
                newName: "ix_topic_experiences_experience_id");

            migrationBuilder.RenameIndex(
                name: "ix_topic_user_id",
                table: "topics",
                newName: "ix_topics_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_topic_resume_id",
                table: "topics",
                newName: "ix_topics_resume_id");

            migrationBuilder.RenameIndex(
                name: "ix_resume_user_id",
                table: "resumes",
                newName: "ix_resumes_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_topic_experiences",
                table: "topic_experiences",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_topics",
                table: "topics",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_resumes",
                table: "resumes",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_experiences_users_user_id",
                table: "experiences",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_resumes_users_user_id",
                table: "resumes",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_tags_users_user_id",
                table: "tags",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_topic_experiences_experiences_experience_id",
                table: "topic_experiences",
                column: "experience_id",
                principalTable: "experiences",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_topic_experiences_topics_topic_id",
                table: "topic_experiences",
                column: "topic_id",
                principalTable: "topics",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_topics_resumes_resume_id",
                table: "topics",
                column: "resume_id",
                principalTable: "resumes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_topics_users_user_id",
                table: "topics",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_experiences_users_user_id",
                table: "experiences");

            migrationBuilder.DropForeignKey(
                name: "fk_resumes_users_user_id",
                table: "resumes");

            migrationBuilder.DropForeignKey(
                name: "fk_tags_users_user_id",
                table: "tags");

            migrationBuilder.DropForeignKey(
                name: "fk_topic_experiences_experiences_experience_id",
                table: "topic_experiences");

            migrationBuilder.DropForeignKey(
                name: "fk_topic_experiences_topics_topic_id",
                table: "topic_experiences");

            migrationBuilder.DropForeignKey(
                name: "fk_topics_resumes_resume_id",
                table: "topics");

            migrationBuilder.DropForeignKey(
                name: "fk_topics_users_user_id",
                table: "topics");

            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_topics",
                table: "topics");

            migrationBuilder.DropPrimaryKey(
                name: "pk_topic_experiences",
                table: "topic_experiences");

            migrationBuilder.DropPrimaryKey(
                name: "pk_resumes",
                table: "resumes");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "user");

            migrationBuilder.RenameTable(
                name: "topics",
                newName: "topic");

            migrationBuilder.RenameTable(
                name: "topic_experiences",
                newName: "topic_experience");

            migrationBuilder.RenameTable(
                name: "resumes",
                newName: "resume");

            migrationBuilder.RenameIndex(
                name: "ix_topics_user_id",
                table: "topic",
                newName: "ix_topic_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_topics_resume_id",
                table: "topic",
                newName: "ix_topic_resume_id");

            migrationBuilder.RenameIndex(
                name: "ix_topic_experiences_topic_id",
                table: "topic_experience",
                newName: "ix_topic_experience_topic_id");

            migrationBuilder.RenameIndex(
                name: "ix_topic_experiences_experience_id",
                table: "topic_experience",
                newName: "ix_topic_experience_experience_id");

            migrationBuilder.RenameIndex(
                name: "ix_resumes_user_id",
                table: "resume",
                newName: "ix_resume_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user",
                table: "user",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_topic",
                table: "topic",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_topic_experience",
                table: "topic_experience",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_resume",
                table: "resume",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_experiences_user_user_id",
                table: "experiences",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_resume_user_user_id",
                table: "resume",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_tags_user_user_id",
                table: "tags",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_topic_resume_resume_id",
                table: "topic",
                column: "resume_id",
                principalTable: "resume",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_topic_user_user_id",
                table: "topic",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_topic_experience_experiences_experience_id",
                table: "topic_experience",
                column: "experience_id",
                principalTable: "experiences",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_topic_experience_topic_topic_id",
                table: "topic_experience",
                column: "topic_id",
                principalTable: "topic",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}