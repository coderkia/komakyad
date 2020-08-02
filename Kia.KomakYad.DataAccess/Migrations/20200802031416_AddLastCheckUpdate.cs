using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kia.KomakYad.DataAccess.Migrations
{
    public partial class AddLastCheckUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateCheck",
                table: "ReadCollections",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdateCheck",
                table: "ReadCollections");
        }
    }
}
