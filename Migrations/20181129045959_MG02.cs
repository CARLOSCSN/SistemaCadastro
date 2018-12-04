using Microsoft.EntityFrameworkCore.Migrations;

namespace SistemaCadastro.Migrations
{
    public partial class MG02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoginNome",
                table: "Item",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginNome",
                table: "Item");
        }
    }
}
