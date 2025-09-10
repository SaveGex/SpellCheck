using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Changemoduleinteractbetweenotherpiecesofapp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "User_Id",
                table: "Words",
                newName: "Author_Id");

            migrationBuilder.RenameColumn(
                name: "Difficulty",
                table: "Words",
                newName: "DifficultyId");

            migrationBuilder.RenameIndex(
                name: "IX_Words_User_Id",
                table: "Words",
                newName: "IX_Words_Author_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Words_Difficulty",
                table: "Words",
                newName: "IX_Words_DifficultyId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_At",
                table: "Modules",
                type: "smalldatetime",
                nullable: false,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<Guid>(
                name: "Identifier",
                table: "Modules",
                type: "uniqueidentifier",
                maxLength: 256,
                nullable: false,
                defaultValueSql: "(newid())");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Modules",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ModuleUser",
                columns: table => new
                {
                    UserModulesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleUser", x => new { x.UserModulesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ModuleUser_Modules_UserModulesId",
                        column: x => x.UserModulesId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModuleUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Manager");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_Identifier",
                table: "Modules",
                column: "Identifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModuleUser_UsersId",
                table: "ModuleUser",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModuleUser");

            migrationBuilder.DropIndex(
                name: "IX_Modules_Identifier",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Created_At",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Modules");

            migrationBuilder.RenameColumn(
                name: "DifficultyId",
                table: "Words",
                newName: "Difficulty");

            migrationBuilder.RenameColumn(
                name: "Author_Id",
                table: "Words",
                newName: "User_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Words_DifficultyId",
                table: "Words",
                newName: "IX_Words_Difficulty");

            migrationBuilder.RenameIndex(
                name: "IX_Words_Author_Id",
                table: "Words",
                newName: "IX_Words_User_Id");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Moderator");
        }
    }
}
