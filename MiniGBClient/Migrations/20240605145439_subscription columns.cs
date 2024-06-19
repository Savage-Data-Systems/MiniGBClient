using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniGBClient.Migrations
{
    /// <inheritdoc />
    public partial class subscriptioncolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "Subscriptions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AuthorizationURI",
                table: "Subscriptions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomerResourceURI",
                table: "Subscriptions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Subscriptions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResourceURI",
                table: "Subscriptions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "AuthorizationURI",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "CustomerResourceURI",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "ResourceURI",
                table: "Subscriptions");
        }
    }
}
