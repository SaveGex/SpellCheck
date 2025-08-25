using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Changesrelatedwithmodulesidentifier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Identifier_Name",
                table: "Modules",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_Identifier_Name_NotNull",
                table: "Modules",
                column: "Identifier_Name",
                unique: true,
                filter: "([Identifier_Name] IS NOT NULL)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Modules_Identifier_Name_NotNull",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Identifier_Name",
                table: "Modules");
        }
    }
}
