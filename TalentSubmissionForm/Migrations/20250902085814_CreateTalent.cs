using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentSubmissionForm.Migrations
{
    /// <inheritdoc />
    public partial class CreateTalent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "talentUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SocialInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OtherInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Interest = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Createdby = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Createdon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Updatedby = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ipdatedon = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_talentUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "uploadedFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Createdby = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Createdon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Updatedby = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ipdatedon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TalentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uploadedFiles", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "talentUsers");

            migrationBuilder.DropTable(
                name: "uploadedFiles");
        }
    }
}
