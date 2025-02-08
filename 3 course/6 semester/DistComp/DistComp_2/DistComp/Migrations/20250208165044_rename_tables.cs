using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DistComp.Migrations
{
    /// <inheritdoc />
    public partial class rename_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notices_Stories_StoryId",
                table: "Notices");

            migrationBuilder.DropForeignKey(
                name: "FK_Stories_Users_UserId",
                table: "Stories");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryTag_Stories_StoriesId",
                table: "StoryTag");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryTag_Tags_TagsId",
                table: "StoryTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stories",
                table: "Stories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notices",
                table: "Notices");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "tbl_user");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "tbl_tag");

            migrationBuilder.RenameTable(
                name: "Stories",
                newName: "tbl_story");

            migrationBuilder.RenameTable(
                name: "Notices",
                newName: "tbl_notice");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Login",
                table: "tbl_user",
                newName: "IX_tbl_user_Login");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_Name",
                table: "tbl_tag",
                newName: "IX_tbl_tag_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Stories_UserId",
                table: "tbl_story",
                newName: "IX_tbl_story_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Stories_Title",
                table: "tbl_story",
                newName: "IX_tbl_story_Title");

            migrationBuilder.RenameIndex(
                name: "IX_Notices_StoryId",
                table: "tbl_notice",
                newName: "IX_tbl_notice_StoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_user",
                table: "tbl_user",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_tag",
                table: "tbl_tag",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_story",
                table: "tbl_story",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_notice",
                table: "tbl_notice",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoryTag_tbl_story_StoriesId",
                table: "StoryTag",
                column: "StoriesId",
                principalTable: "tbl_story",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryTag_tbl_tag_TagsId",
                table: "StoryTag",
                column: "TagsId",
                principalTable: "tbl_tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_notice_tbl_story_StoryId",
                table: "tbl_notice",
                column: "StoryId",
                principalTable: "tbl_story",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_story_tbl_user_UserId",
                table: "tbl_story",
                column: "UserId",
                principalTable: "tbl_user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoryTag_tbl_story_StoriesId",
                table: "StoryTag");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryTag_tbl_tag_TagsId",
                table: "StoryTag");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_notice_tbl_story_StoryId",
                table: "tbl_notice");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_story_tbl_user_UserId",
                table: "tbl_story");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_user",
                table: "tbl_user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_tag",
                table: "tbl_tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_story",
                table: "tbl_story");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_notice",
                table: "tbl_notice");

            migrationBuilder.RenameTable(
                name: "tbl_user",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "tbl_tag",
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "tbl_story",
                newName: "Stories");

            migrationBuilder.RenameTable(
                name: "tbl_notice",
                newName: "Notices");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_user_Login",
                table: "Users",
                newName: "IX_Users_Login");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_tag_Name",
                table: "Tags",
                newName: "IX_Tags_Name");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_story_UserId",
                table: "Stories",
                newName: "IX_Stories_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_story_Title",
                table: "Stories",
                newName: "IX_Stories_Title");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_notice_StoryId",
                table: "Notices",
                newName: "IX_Notices_StoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stories",
                table: "Stories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notices",
                table: "Notices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notices_Stories_StoryId",
                table: "Notices",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_Users_UserId",
                table: "Stories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryTag_Stories_StoriesId",
                table: "StoryTag",
                column: "StoriesId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryTag_Tags_TagsId",
                table: "StoryTag",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
