using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CA.Repositorios.Migrations
{
    public partial class remocao_obrigatoriedade_id_apontamento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IdApontamentoTfs",
                table: "ApontamentoChannel",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IdApontamentoTfs",
                table: "ApontamentoChannel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
