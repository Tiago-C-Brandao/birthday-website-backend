using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BirthdayWebsiteAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddGiftImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageLink",
                table: "Gifts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageLink",
                table: "Gifts");
        }
    }
}
