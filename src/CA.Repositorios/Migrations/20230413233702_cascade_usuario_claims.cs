using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CA.Repositorios.Migrations
{
    public partial class cascade_usuario_claims : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioClaims_Usuario_UserId",
                table: "UsuarioClaims");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioClaims_Usuario_UserId",
                table: "UsuarioClaims",
                column: "UserId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioClaims_Usuario_UserId",
                table: "UsuarioClaims");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioClaims_Usuario_UserId",
                table: "UsuarioClaims",
                column: "UserId",
                principalTable: "Usuario",
                principalColumn: "Id");
        }
    }
}
