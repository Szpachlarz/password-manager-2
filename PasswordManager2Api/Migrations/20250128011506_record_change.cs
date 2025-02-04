using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager2Api.Migrations
{
    /// <inheritdoc />
    public partial class record_change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Records",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Records",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Records");
        }
    }
}
