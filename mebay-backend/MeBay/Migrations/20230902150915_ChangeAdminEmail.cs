using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeBay.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAdminEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationDate", "Email", "ModificationDate", "Password" },
                values: new object[] { new DateTime(2023, 9, 2, 15, 9, 15, 424, DateTimeKind.Utc).AddTicks(360), "admin@mebay.com", new DateTime(2023, 9, 2, 15, 9, 15, 424, DateTimeKind.Utc).AddTicks(360), "$2a$11$BCVExsrNkyEleihbFl4Tx.ADvWaxX3oCpFi8KKFaMQxOaDG/KmZ4W" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationDate", "Email", "ModificationDate", "Password" },
                values: new object[] { new DateTime(2023, 9, 2, 15, 6, 31, 293, DateTimeKind.Utc).AddTicks(7020), "admin@fox.hu", new DateTime(2023, 9, 2, 15, 6, 31, 293, DateTimeKind.Utc).AddTicks(7020), "$2a$11$wxL5UrRW2eU.sXhJObHfd.fIITLQci3A8z1lIbItp7mwc5TZafn9a" });
        }
    }
}
