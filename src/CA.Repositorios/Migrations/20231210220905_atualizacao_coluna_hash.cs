using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CA.Repositorios.Migrations
{
    public partial class atualizacao_coluna_hash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioCA_UsuarioCA_IdGerente",
                table: "UsuarioCA");

            migrationBuilder.RenameColumn(
                name: "Hash",
                table: "ApontamentoChannel",
                newName: "IdApontamentoTfs");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioCA_UsuarioCA_IdGerente",
                table: "UsuarioCA",
                column: "IdGerente",
                principalTable: "UsuarioCA",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioCA_UsuarioCA_IdGerente",
                table: "UsuarioCA");

            migrationBuilder.RenameColumn(
                name: "IdApontamentoTfs",
                table: "ApontamentoChannel",
                newName: "Hash");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioCA_UsuarioCA_IdGerente",
                table: "UsuarioCA",
                column: "IdGerente",
                principalTable: "UsuarioCA",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
