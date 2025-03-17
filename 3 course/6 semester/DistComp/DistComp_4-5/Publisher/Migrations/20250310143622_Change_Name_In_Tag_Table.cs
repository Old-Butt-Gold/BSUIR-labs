using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Publisher.Migrations
{
    /// <inheritdoc />
    public partial class Change_Name_In_Tag_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "tbl_tag",
                newName: "name");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_tag_Name",
                table: "tbl_tag",
                newName: "IX_tbl_tag_name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "tbl_tag",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_tag_name",
                table: "tbl_tag",
                newName: "IX_tbl_tag_Name");
        }
    }
}
