using Microsoft.EntityFrameworkCore.Migrations;

namespace Kia.KomakYad.DataAccess.Migrations
{
    public partial class addQACard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "Cards");

            migrationBuilder.AddColumn<string>(
                name: "Answer",
                table: "Cards",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Example",
                table: "Cards",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraData",
                table: "Cards",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Question",
                table: "Cards",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Example",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "ExtraData",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Question",
                table: "Cards");

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
