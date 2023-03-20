using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdeDl.BlazorApp.Migrations
{
    /// <inheritdoc />
    public partial class Versamenti : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Versamenti",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PeriodYear = table.Column<int>(type: "INTEGER", nullable: true),
                    PeriodFrom = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PeriodTo = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Ente = table.Column<string>(type: "TEXT", nullable: true),
                    CodiceTributo1 = table.Column<string>(type: "TEXT", nullable: true),
                    CodiceTributo2 = table.Column<string>(type: "TEXT", nullable: true),
                    CodiceTributo3 = table.Column<string>(type: "TEXT", nullable: true),
                    CodiceTributo4 = table.Column<string>(type: "TEXT", nullable: true),
                    Prefisso = table.Column<string>(type: "TEXT", nullable: true),
                    Credito = table.Column<bool>(type: "INTEGER", nullable: false),
                    NoAddizionale = table.Column<bool>(type: "INTEGER", nullable: false),
                    Coobbligato = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Versamenti", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Versamenti");
        }
    }
}
