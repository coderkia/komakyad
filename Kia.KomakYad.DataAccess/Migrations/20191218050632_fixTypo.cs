using Microsoft.EntityFrameworkCore.Migrations;

namespace Kia.KomakYad.DataAccess.Migrations
{
    public partial class fixTypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviouseDeck",
                table: "DueCards");

            migrationBuilder.AlterColumn<byte>(
                name: "ReadPerDay",
                table: "UserCollections",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<byte>(
                name: "PreviousDeck",
                table: "DueCards",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousDeck",
                table: "DueCards");

            migrationBuilder.AlterColumn<int>(
                name: "ReadPerDay",
                table: "UserCollections",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.AddColumn<byte>(
                name: "PreviouseDeck",
                table: "DueCards",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
