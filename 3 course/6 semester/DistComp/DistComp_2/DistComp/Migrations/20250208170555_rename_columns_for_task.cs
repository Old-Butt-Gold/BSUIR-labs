using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DistComp.Migrations
{
    /// <inheritdoc />
    public partial class rename_columns_for_task : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_story_tbl_user_UserId",
                table: "tbl_story");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tbl_user",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tbl_tag",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tbl_story",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "tbl_story",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_story_UserId",
                table: "tbl_story",
                newName: "IX_tbl_story_user_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tbl_notice",
                newName: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_story_tbl_user_user_id",
                table: "tbl_story",
                column: "user_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_story_tbl_user_user_id",
                table: "tbl_story");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tbl_user",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tbl_tag",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tbl_story",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "tbl_story",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_story_user_id",
                table: "tbl_story",
                newName: "IX_tbl_story_UserId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tbl_notice",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_story_tbl_user_UserId",
                table: "tbl_story",
                column: "UserId",
                principalTable: "tbl_user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
