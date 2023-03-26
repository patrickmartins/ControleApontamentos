using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CA.Repositorios.Migrations
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
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClaimType = table.Column<string>(type: "varchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "varchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "varchar(256)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", nullable: true),
                    Email = table.Column<string>(type: "varchar(256)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "varchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioChannel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    NomeUsuario = table.Column<string>(type: "varchar(50)", nullable: false),
                    NomeCompleto = table.Column<string>(type: "varchar(200)", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioChannel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "varchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioRoles", x => new { x.RoleId, x.UserId });
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
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(450)", nullable: false),
                    UsuarioId = table.Column<string>(type: "varchar(450)", nullable: true),
                    Name = table.Column<string>(type: "varchar(450)", nullable: false),
                    NormalizedName = table.Column<string>(type: "varchar(450)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UsuarioClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "varchar(450)", nullable: true),
                    ClaimType = table.Column<string>(type: "varchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "varchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioClaims_Usuario_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "Id");
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

            migrationBuilder.CreateIndex(
                name: "IX_Role_UsuarioId",
                table: "Role",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioClaims_UserId",
                table: "UsuarioClaims",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApontamentoChannel");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "UsuarioClaims");

            migrationBuilder.DropTable(
                name: "UsuarioRoles");

            migrationBuilder.DropTable(
                name: "AtividadeChannel");

            migrationBuilder.DropTable(
                name: "UsuarioChannel");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "ProjetoChannel");
        }
    }
}
