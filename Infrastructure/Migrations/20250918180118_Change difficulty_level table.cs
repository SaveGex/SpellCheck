using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainData.Migrations
{
    /// <inheritdoc />
    public partial class Changedifficulty_leveltable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Difficulty",
                table: "Difficulty_Level",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Difficulty_Level",
                table: "Difficulty_Level",
                column: "Difficulty");

            migrationBuilder.AddCheckConstraint(
                name: "CHK_Difficulty_Range",
                table: "Difficulty_Level",
                sql: "Difficulty >= 1 AND Difficulty <= 6");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Difficulty_Level",
                table: "Difficulty_Level");

            migrationBuilder.DropCheckConstraint(
                name: "CHK_Difficulty_Range",
                table: "Difficulty_Level");

            migrationBuilder.AlterColumn<int>(
                name: "Difficulty",
                table: "Difficulty_Level",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
