using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdeDl.BlazorApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Pin",
                table: "Credentials",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pin",
                table: "Credentials");
        }
    }
}
