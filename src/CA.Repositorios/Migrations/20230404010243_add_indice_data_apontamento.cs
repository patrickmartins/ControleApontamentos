using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CA.Repositorios.Migrations
{
    public partial class add_indice_data_apontamento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ApontamentoChannel_Data",
                table: "ApontamentoChannel",
                column: "Data");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApontamentoChannel_Data",
                table: "ApontamentoChannel");
        }
    }
}
