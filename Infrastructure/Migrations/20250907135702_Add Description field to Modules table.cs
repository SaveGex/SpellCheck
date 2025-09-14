using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainData.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionfieldtoModulestable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Modules",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Modules");
        }
    }
}
