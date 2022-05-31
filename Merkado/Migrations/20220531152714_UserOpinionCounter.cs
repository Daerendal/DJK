using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Merkado.Migrations
{
    public partial class UserOpinionCounter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OpinionCounter",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PromoCode",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpinionCounter",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PromoCode",
                table: "Users");
        }
    }
}
