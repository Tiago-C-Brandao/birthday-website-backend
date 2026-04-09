using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BirthdayWebsiteAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddGuestStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Guests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Guests");
        }
    }
}
