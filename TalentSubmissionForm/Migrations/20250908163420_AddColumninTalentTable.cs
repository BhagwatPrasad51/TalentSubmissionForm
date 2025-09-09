using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentSubmissionForm.Migrations
{
    /// <inheritdoc />
    public partial class AddColumninTalentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "talentUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "talentUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "talentUsers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "talentUsers");
        }
    }
}
