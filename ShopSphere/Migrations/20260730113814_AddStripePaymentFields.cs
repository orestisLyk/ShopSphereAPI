using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopSphere.Migrations
{
    /// <inheritdoc />
    public partial class AddStripePaymentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProviderPaymentId",
                table: "Payments",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ProviderPaymentId",
                table: "Payments");
        }
    }
}
