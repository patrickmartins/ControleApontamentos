using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CA.Repositorios.Migrations
{
    public partial class reestruturacao_relatorios_apontamentos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioRoles_Usuario_UserId",
                table: "UsuarioRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioClaims_Usuario_UserId",
                table: "UsuarioClaims");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_UsuarioRoles_RoleId",
                table: "UsuarioRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuarioRoles",
                table: "UsuarioRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuarioClaims",
                table: "UsuarioClaims");

            migrationBuilder.RenameTable(
                name: "UsuarioRoles",
                newName: "UsuarioIdentityRoles");

            migrationBuilder.RenameTable(
                name: "UsuarioClaims",
                newName: "UsuarioIdentityClaims");

            migrationBuilder.RenameIndex(
                name: "IX_UsuarioClaims_UserId",
                table: "UsuarioIdentityClaims",
                newName: "IX_UsuarioIdentityClaims_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "Role",
                type: "varchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UsuarioIdentityClaims",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuarioIdentityRoles",
                table: "UsuarioIdentityRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuarioIdentityClaims",
                table: "UsuarioIdentityClaims",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Unidade",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unidade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioCA",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomeCompleto = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    IdUnidade = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdGerente = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioCA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioCA_Unidade_IdUnidade",
                        column: x => x.IdUnidade,
                        principalTable: "Unidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UsuarioCA_UsuarioCA_IdGerente",
                        column: x => x.IdGerente,
                        principalTable: "UsuarioCA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ParametrosIntegracao",
                columns: table => new
                {
                    IdUsuario = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NomeUsuarioTfs = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    DominioTfs = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    IdUsuarioTfs = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    TipoIdUsuarioTfs = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    IdFuncionarioPonto = table.Column<int>(type: "int", nullable: true),
                    PisFuncionarioPonto = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    IdUsuarioChannel = table.Column<int>(type: "int", nullable: true),
                    NomeUsuarioChannel = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParametrosIntegracao", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_ParametrosIntegracao_UsuarioCA_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "UsuarioCA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioIdentity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioIdentity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioIdentity_UsuarioCA_Id",
                        column: x => x.Id,
                        principalTable: "UsuarioCA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioCA_IdGerente",
                table: "UsuarioCA",
                column: "IdGerente");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioCA_IdUnidade",
                table: "UsuarioCA",
                column: "IdUnidade");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioIdentityClaims_UsuarioIdentity_UserId",
                table: "UsuarioIdentityClaims",
                column: "UserId",
                principalTable: "UsuarioIdentity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioIdentityClaims_UsuarioIdentity_UserId",
                table: "UsuarioIdentityClaims");

            migrationBuilder.DropTable(
                name: "ParametrosIntegracao");

            migrationBuilder.DropTable(
                name: "UsuarioIdentity");

            migrationBuilder.DropTable(
                name: "UsuarioCA");

            migrationBuilder.DropTable(
                name: "Unidade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuarioIdentityRoles",
                table: "UsuarioIdentityRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuarioIdentityClaims",
                table: "UsuarioIdentityClaims");

            migrationBuilder.RenameTable(
                name: "UsuarioIdentityRoles",
                newName: "UsuarioRoles");

            migrationBuilder.RenameTable(
                name: "UsuarioIdentityClaims",
                newName: "UsuarioClaims");

            migrationBuilder.RenameIndex(
                name: "IX_UsuarioIdentityClaims_UserId",
                table: "UsuarioClaims",
                newName: "IX_UsuarioClaims_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "Role",
                type: "varchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "UsuarioRoles",
                type: "varchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UsuarioClaims",
                type: "varchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuarioRoles",
                table: "UsuarioRoles",
                columns: new[] { "RoleId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuarioClaims",
                table: "UsuarioClaims",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(450)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(max)", nullable: true),
                    Email = table.Column<string>(type: "varchar(256)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", nullable: true),
                    PasswordHash = table.Column<string>(type: "varchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "varchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "varchar(256)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Role_UsuarioId",
                table: "UsuarioRoles",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioRoles_Usuario_UserId",
                table: "UsuarioRoles",
                column: "UserId",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioClaims_Usuario_UserId",
                table: "UsuarioClaims",
                column: "UserId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
