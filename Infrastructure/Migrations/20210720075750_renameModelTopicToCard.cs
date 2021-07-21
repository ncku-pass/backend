using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class renameModelTopicToCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_card_experiences_cards_topic_id",
                table: "card_experiences");

            migrationBuilder.DropIndex(
                name: "ix_card_experiences_topic_id",
                table: "card_experiences");

            migrationBuilder.DropColumn(
                name: "topic_id",
                table: "card_experiences");

            migrationBuilder.CreateIndex(
                name: "ix_card_experiences_card_id",
                table: "card_experiences",
                column: "card_id");

            migrationBuilder.AddForeignKey(
                name: "fk_card_experiences_cards_card_id",
                table: "card_experiences",
                column: "card_id",
                principalTable: "cards",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_card_experiences_cards_card_id",
                table: "card_experiences");

            migrationBuilder.DropIndex(
                name: "ix_card_experiences_card_id",
                table: "card_experiences");

            migrationBuilder.AddColumn<int>(
                name: "topic_id",
                table: "card_experiences",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_card_experiences_topic_id",
                table: "card_experiences",
                column: "topic_id");

            migrationBuilder.AddForeignKey(
                name: "fk_card_experiences_cards_topic_id",
                table: "card_experiences",
                column: "topic_id",
                principalTable: "cards",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
