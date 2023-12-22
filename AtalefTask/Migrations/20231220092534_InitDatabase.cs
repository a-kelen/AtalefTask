using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtalefTask.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SmartMatchResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UniqueValue = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartMatchResult", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SmartMatchResult_UniqueValue",
                table: "SmartMatchResult",
                column: "UniqueValue",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SmartMatchResult_UserId",
                table: "SmartMatchResult",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmartMatchResult");
        }
    }
}
