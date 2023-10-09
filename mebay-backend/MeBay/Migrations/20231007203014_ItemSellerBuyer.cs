using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeBay.Migrations
{
    /// <inheritdoc />
    public partial class ItemSellerBuyer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Users_UserId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Items",
                newName: "SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_UserId",
                table: "Items",
                newName: "IX_Items_SellerId");

            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_BuyerId",
                table: "Items",
                column: "BuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Users_BuyerId",
                table: "Items",
                column: "BuyerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Users_SellerId",
                table: "Items",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Users_BuyerId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Users_SellerId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_BuyerId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "Items",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_SellerId",
                table: "Items",
                newName: "IX_Items_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Users_UserId",
                table: "Items",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
