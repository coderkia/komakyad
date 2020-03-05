using Microsoft.EntityFrameworkCore.Migrations;

namespace Kia.KomakYad.DataAccess.Migrations
{
    public partial class AddIsPrivateCollection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Collections",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Collections");
        }
    }
}
