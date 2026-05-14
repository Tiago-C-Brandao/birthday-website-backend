using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BirthdayWebsiteAPI.Migrations
{
    /// <inheritdoc />
    public partial class AdjustGiftTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Gifts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Gifts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Rarity",
                table: "Gifts",
                type: "int",
                nullable: true,
                defaultValue: 1);

            migrationBuilder.Sql("UPDATE Gifts SET Rarity = 1 WHERE Rarity IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "Rarity",
                table: "Gifts");
        }
    }
}
