using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Publisher.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Notice_Model_For_Lab_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_notice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_notice",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StoryId = table.Column<long>(type: "bigint", nullable: false),
                    Content = table.Column<string>(type: "text", maxLength: 2048, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_notice", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_notice_tbl_story_StoryId",
                        column: x => x.StoryId,
                        principalTable: "tbl_story",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_notice_StoryId",
                table: "tbl_notice",
                column: "StoryId");
        }
    }
}
