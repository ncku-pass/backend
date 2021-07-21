using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class refactorAndRenameTopicToCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "pk_topics",
                table: "topics");

            migrationBuilder.DropPrimaryKey(
                name: "pk_topic_experiences",
                table: "topic_experiences");

            migrationBuilder.RenameTable(
                name: "topics",
                newName: "cards");

            migrationBuilder.RenameTable(
                name: "topic_experiences",
                newName: "card_experiences");

            migrationBuilder.RenameIndex(
                name: "ix_topics_user_id",
                table: "cards",
                newName: "ix_cards_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_topics_resume_id",
                table: "cards",
                newName: "ix_cards_resume_id");

            migrationBuilder.RenameIndex(
                name: "ix_topic_experiences_topic_id",
                table: "card_experiences",
                newName: "ix_card_experiences_topic_id");

            migrationBuilder.RenameIndex(
                name: "ix_topic_experiences_experience_id",
                table: "card_experiences",
                newName: "ix_card_experiences_experience_id");

            migrationBuilder.AddColumn<int>(
                name: "card_type",
                table: "cards",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "cards",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "order",
                table: "cards",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "topic_id",
                table: "card_experiences",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "card_id",
                table: "card_experiences",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "show_feedback",
                table: "card_experiences",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "show_position",
                table: "card_experiences",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "pk_cards",
                table: "cards",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_card_experiences",
                table: "card_experiences",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_card_experiences_experiences_experience_id",
                table: "card_experiences",
                column: "experience_id",
                principalTable: "experiences",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_card_experiences_cards_topic_id",
                table: "card_experiences",
                column: "topic_id",
                principalTable: "cards",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_cards_resumes_resume_id",
                table: "cards",
                column: "resume_id",
                principalTable: "resumes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_cards_users_user_id",
                table: "cards",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_card_experiences_experiences_experience_id",
                table: "card_experiences");

            migrationBuilder.DropForeignKey(
                name: "fk_card_experiences_cards_topic_id",
                table: "card_experiences");

            migrationBuilder.DropForeignKey(
                name: "fk_cards_resumes_resume_id",
                table: "cards");

            migrationBuilder.DropForeignKey(
                name: "fk_cards_users_user_id",
                table: "cards");

            migrationBuilder.DropPrimaryKey(
                name: "pk_cards",
                table: "cards");

            migrationBuilder.DropPrimaryKey(
                name: "pk_card_experiences",
                table: "card_experiences");

            migrationBuilder.DropColumn(
                name: "card_type",
                table: "cards");

            migrationBuilder.DropColumn(
                name: "description",
                table: "cards");

            migrationBuilder.DropColumn(
                name: "order",
                table: "cards");

            migrationBuilder.DropColumn(
                name: "card_id",
                table: "card_experiences");

            migrationBuilder.DropColumn(
                name: "show_feedback",
                table: "card_experiences");

            migrationBuilder.DropColumn(
                name: "show_position",
                table: "card_experiences");

            migrationBuilder.RenameTable(
                name: "cards",
                newName: "topics");

            migrationBuilder.RenameTable(
                name: "card_experiences",
                newName: "topic_experiences");

            migrationBuilder.RenameIndex(
                name: "ix_cards_user_id",
                table: "topics",
                newName: "ix_topics_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_cards_resume_id",
                table: "topics",
                newName: "ix_topics_resume_id");

            migrationBuilder.RenameIndex(
                name: "ix_card_experiences_topic_id",
                table: "topic_experiences",
                newName: "ix_topic_experiences_topic_id");

            migrationBuilder.RenameIndex(
                name: "ix_card_experiences_experience_id",
                table: "topic_experiences",
                newName: "ix_topic_experiences_experience_id");

            migrationBuilder.AlterColumn<int>(
                name: "topic_id",
                table: "topic_experiences",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_topics",
                table: "topics",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_topic_experiences",
                table: "topic_experiences",
                column: "id");

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
    }
}
