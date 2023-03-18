using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdeDl.BlazorApp.Migrations
{
    /// <inheritdoc />
    public partial class FixedForeignkeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Credentials_CredentialId",
                table: "Customers");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Credentials_CredentialId",
                table: "Customers",
                column: "CredentialId",
                principalTable: "Credentials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Credentials_CredentialId",
                table: "Customers");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Credentials_CredentialId",
                table: "Customers",
                column: "CredentialId",
                principalTable: "Credentials",
                principalColumn: "Id");
        }
    }
}
