using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentSubmissionForm.Migrations
{
    /// <inheritdoc />
    public partial class ChangefieldName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
               name: "Hieght",
               table: "talentUsers",
               newName: "Height");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
               name: "Height",
               table: "talentUsers",
               newName: "Hieght");
        }
    }
}
