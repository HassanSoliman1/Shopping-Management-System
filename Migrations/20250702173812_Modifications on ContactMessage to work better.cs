using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceAPI.Migrations
{
    /// <inheritdoc />
    public partial class ModificationsonContactMessagetoworkbetter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ContactMessages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReplyedAt",
                table: "ContactMessages",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "RepliedById",
                table: "ContactMessages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SendById",
                table: "ContactMessages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMessages_RepliedById",
                table: "ContactMessages",
                column: "RepliedById");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMessages_SendById",
                table: "ContactMessages",
                column: "SendById");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactMessages_AspNetUsers_RepliedById",
                table: "ContactMessages",
                column: "RepliedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactMessages_AspNetUsers_SendById",
                table: "ContactMessages",
                column: "SendById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactMessages_AspNetUsers_RepliedById",
                table: "ContactMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactMessages_AspNetUsers_SendById",
                table: "ContactMessages");

            migrationBuilder.DropIndex(
                name: "IX_ContactMessages_RepliedById",
                table: "ContactMessages");

            migrationBuilder.DropIndex(
                name: "IX_ContactMessages_SendById",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "RepliedById",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "SendById",
                table: "ContactMessages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReplyedAt",
                table: "ContactMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ContactMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ContactMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
