using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager2Api.Migrations
{
    /// <inheritdoc />
    public partial class field_name_change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServiceName",
                table: "Records",
                newName: "Website");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Website",
                table: "Records",
                newName: "ServiceName");
        }
    }
}
