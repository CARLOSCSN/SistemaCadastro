using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SistemaCadastro.Migrations
{
    public partial class MG01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_LoginViewModel_LoginID",
                table: "Item");

            migrationBuilder.DropTable(
                name: "LoginViewModel");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Login",
                newName: "LoginID");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Login",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Login",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Login_LoginID",
                table: "Item",
                column: "LoginID",
                principalTable: "Login",
                principalColumn: "LoginID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Login_LoginID",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "LoginID",
                table: "Login",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Login",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Login",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateTable(
                name: "LoginViewModel",
                columns: table => new
                {
                    LoginID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Password = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginViewModel", x => x.LoginID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Item_LoginViewModel_LoginID",
                table: "Item",
                column: "LoginID",
                principalTable: "LoginViewModel",
                principalColumn: "LoginID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
