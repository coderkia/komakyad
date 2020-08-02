using Microsoft.EntityFrameworkCore.Migrations;

namespace Kia.KomakYad.DataAccess.Migrations
{
    public partial class RenameExtraDataInCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtraData",
                table: "Cards");

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "Cards",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "Cards");

            migrationBuilder.AddColumn<string>(
                name: "ExtraData",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
