using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentSubmissionForm.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFileTableColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ipdatedon",
                table: "uploadedFiles",
                newName: "Updatedon");

            migrationBuilder.RenameColumn(
                name: "Deletedon",
                table: "uploadedFiles",
                newName: "Isdeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Updatedon",
                table: "uploadedFiles",
                newName: "Ipdatedon");

            migrationBuilder.RenameColumn(
                name: "Isdeleted",
                table: "uploadedFiles",
                newName: "Deletedon");
        }
    }
}
