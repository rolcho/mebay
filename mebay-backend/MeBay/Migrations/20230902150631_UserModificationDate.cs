using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeBay.Migrations
{
    /// <inheritdoc />
    public partial class UserModificationDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "Users",
                newName: "ModificationDate");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationDate", "ModificationDate", "Password" },
                values: new object[] { new DateTime(2023, 9, 2, 15, 6, 31, 293, DateTimeKind.Utc).AddTicks(7020), new DateTime(2023, 9, 2, 15, 6, 31, 293, DateTimeKind.Utc).AddTicks(7020), "$2a$11$wxL5UrRW2eU.sXhJObHfd.fIITLQci3A8z1lIbItp7mwc5TZafn9a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModificationDate",
                table: "Users",
                newName: "UpdateDate");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationDate", "Password", "UpdateDate" },
                values: new object[] { new DateTime(2023, 9, 2, 12, 57, 7, 994, DateTimeKind.Utc).AddTicks(6680), "$2a$11$e9ZvTjrMUUnMytFBBdVwvODnxYhaqLFXtzQrDlJfUE6PmMQHgjgrq", new DateTime(2023, 9, 2, 12, 57, 7, 994, DateTimeKind.Utc).AddTicks(6690) });
        }
    }
}
