using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentSubmissionForm.Migrations
{
    /// <inheritdoc />
    public partial class AddNewColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                table: "uploadedFiles");

            migrationBuilder.RenameColumn(
                name: "Action",
                table: "uploadedFiles",
                newName: "Size");

            migrationBuilder.RenameColumn(
                name: "updatedon",
                table: "talentUsers",
                newName: "Updatedon");

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "uploadedFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                table: "uploadedFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Deletedon",
                table: "talentUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "talentUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extension",
                table: "uploadedFiles");

            migrationBuilder.DropColumn(
                name: "MimeType",
                table: "uploadedFiles");

            migrationBuilder.DropColumn(
                name: "Deletedon",
                table: "talentUsers");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "talentUsers");

            migrationBuilder.RenameColumn(
                name: "Size",
                table: "uploadedFiles",
                newName: "Action");

            migrationBuilder.RenameColumn(
                name: "Updatedon",
                table: "talentUsers",
                newName: "updatedon");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "uploadedFiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
