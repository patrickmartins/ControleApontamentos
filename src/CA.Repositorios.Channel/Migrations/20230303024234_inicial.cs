using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CA.Repositorios.Channel.Migrations
{
    public partial class inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjetoChannel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "varchar(200)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetoChannel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioChannel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    NomeCompleto = table.Column<string>(type: "varchar(200)", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioChannel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AtividadeChannel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "varchar(200)", nullable: false),
                    Codigo = table.Column<string>(type: "varchar(30)", nullable: false),
                    ProjetoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtividadeChannel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtividadeChannel_ProjetoChannel_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "ProjetoChannel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApontamentoChannel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    IdTarefaTfs = table.Column<int>(type: "int", nullable: true),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    ApontamentoTfs = table.Column<bool>(type: "bit", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comentario = table.Column<string>(type: "varchar(2000)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TempoApontado = table.Column<TimeSpan>(type: "time", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    ProjetoId = table.Column<int>(type: "int", nullable: true),
                    AtividadeId = table.Column<int>(type: "int", nullable: true),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApontamentoChannel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApontamentoChannel_AtividadeChannel_AtividadeId",
                        column: x => x.AtividadeId,
                        principalTable: "AtividadeChannel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApontamentoChannel_ProjetoChannel_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "ProjetoChannel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApontamentoChannel_UsuarioChannel_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "UsuarioChannel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApontamentoChannel_AtividadeId",
                table: "ApontamentoChannel",
                column: "AtividadeId");

            migrationBuilder.CreateIndex(
                name: "IX_ApontamentoChannel_ProjetoId",
                table: "ApontamentoChannel",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ApontamentoChannel_UsuarioId",
                table: "ApontamentoChannel",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_AtividadeChannel_ProjetoId",
                table: "AtividadeChannel",
                column: "ProjetoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApontamentoChannel");

            migrationBuilder.DropTable(
                name: "AtividadeChannel");

            migrationBuilder.DropTable(
                name: "UsuarioChannel");

            migrationBuilder.DropTable(
                name: "ProjetoChannel");
        }
    }
}
