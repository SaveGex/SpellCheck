using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainData.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Difficulty_Level",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Difficul__3214EC071CD4BFD4", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Size = table.Column<int>(type: "int", nullable: true),
                    EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "smalldatetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Files__3214EC0755E9AEE8", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true),
                    Created_At = table.Column<DateTime>(type: "smalldatetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__3214EC07EF14166B", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    from_individual_id = table.Column<int>(type: "int", nullable: false),
                    to_individual_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Friends__3214EC0787ADD9A2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_from_individual_Friend_id_Users",
                        column: x => x.from_individual_id,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_to_individual_Friend_id_Users",
                        column: x => x.to_individual_id,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Modules__3214EC079E5E094A", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modules_Users",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Module_Id = table.Column<int>(type: "int", nullable: false),
                    Expression = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Meaning = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: true),
                    Created_At = table.Column<DateTime>(type: "smalldatetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Words__3214EC07AEB353A1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Difficulty_Difficulty_Level",
                        column: x => x.Difficulty,
                        principalTable: "Difficulty_Level",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Words_Modules",
                        column: x => x.Module_Id,
                        principalTable: "Modules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Words_Users",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friends_to_individual_id",
                table: "Friends",
                column: "to_individual_id");

            migrationBuilder.CreateIndex(
                name: "UQ_from_individual_to_individual_ids",
                table: "Friends",
                columns: new[] { "from_individual_id", "to_individual_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_User_Id",
                table: "Modules",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email_NotNull",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "([Email] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Number_NotNull",
                table: "Users",
                column: "Number",
                unique: true,
                filter: "([Number] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_Words_Difficulty",
                table: "Words",
                column: "Difficulty");

            migrationBuilder.CreateIndex(
                name: "IX_Words_Module_Id",
                table: "Words",
                column: "Module_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Words_User_Id",
                table: "Words",
                column: "User_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Friends");

            migrationBuilder.DropTable(
                name: "Words");

            migrationBuilder.DropTable(
                name: "Difficulty_Level");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
