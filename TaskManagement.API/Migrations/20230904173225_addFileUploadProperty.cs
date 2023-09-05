using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class addFileUploadProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttachFile",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachFile",
                table: "Tasks");
        }
    }
}
