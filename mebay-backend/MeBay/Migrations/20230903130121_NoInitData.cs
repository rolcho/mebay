using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeBay.Migrations
{
    /// <inheritdoc />
    public partial class NoInitData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreationDate", "Email", "ModificationDate", "Name", "Password", "Role" },
                values: new object[] { 1, new DateTime(2023, 9, 2, 15, 9, 15, 424, DateTimeKind.Utc).AddTicks(360), "admin@mebay.com", new DateTime(2023, 9, 2, 15, 9, 15, 424, DateTimeKind.Utc).AddTicks(360), "admin", "$2a$11$BCVExsrNkyEleihbFl4Tx.ADvWaxX3oCpFi8KKFaMQxOaDG/KmZ4W", "Admin" });
        }
    }
}
