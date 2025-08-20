using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace To_Do.API.Migrations
{
    /// <inheritdoc />
    public partial class AuthTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ToDoItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ToDoItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ToDoItems",
                keyColumn: "Id",
                keyValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ToDoItems",
                columns: new[] { "Id", "CreatedAt", "Description", "IsCompleted", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 8, 2, 12, 0, 0, 0, DateTimeKind.Utc), "Create new Task.", false, "Add POST method" },
                    { 2, new DateTime(2025, 8, 2, 12, 0, 0, 0, DateTimeKind.Utc), "Update existing task.", false, "Add PUT method" },
                    { 3, new DateTime(2025, 8, 2, 12, 0, 0, 0, DateTimeKind.Utc), "Remove existing task.", false, "Add DELETE method" }
                });
        }
    }
}
