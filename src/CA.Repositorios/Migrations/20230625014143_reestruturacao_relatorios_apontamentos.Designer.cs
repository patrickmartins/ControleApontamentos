﻿// <auto-generated />
using System;
using CA.Repositorios.Contexto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CA.Repositorios.Migrations
{
    [DbContext(typeof(ContextoDadosCA))]
    [Migration("20230625014143_reestruturacao_relatorios_apontamentos")]
    partial class reestruturacao_relatorios_apontamentos
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CA.Core.Entidades.CA.ParametrosIntegracao", b =>
                {
                    b.Property<string>("IdUsuario")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DominioTfs")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<int?>("IdFuncionarioPonto")
                        .HasColumnType("int");

                    b.Property<int?>("IdUsuarioChannel")
                        .HasColumnType("int");

                    b.Property<string>("IdUsuarioTfs")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("NomeUsuarioChannel")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("NomeUsuarioTfs")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("PisFuncionarioPonto")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("TipoIdUsuarioTfs")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.HasKey("IdUsuario");

                    b.ToTable("ParametrosIntegracao");
                });

            modelBuilder.Entity("CA.Core.Entidades.CA.Unidade", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Unidade");
                });

            modelBuilder.Entity("CA.Core.Entidades.CA.UsuarioCA", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdGerente")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdUnidade")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NomeCompleto")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("IdGerente");

                    b.HasIndex("IdUnidade");

                    b.ToTable("UsuarioCA");
                });

            modelBuilder.Entity("CA.Core.Entidades.Channel.ApontamentoChannel", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<bool>("ApontamentoTfs")
                        .HasColumnType("bit");

                    b.Property<int?>("AtividadeId")
                        .HasColumnType("int");

                    b.Property<string>("Comentario")
                        .IsRequired()
                        .HasColumnType("varchar(5000)");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IdTarefaTfs")
                        .HasColumnType("int");

                    b.Property<int?>("ProjetoId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("TempoApontado")
                        .HasColumnType("time");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AtividadeId");

                    b.HasIndex("Data");

                    b.HasIndex("ProjetoId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("ApontamentoChannel");
                });

            modelBuilder.Entity("CA.Core.Entidades.Channel.AtividadeChannel", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<int>("ProjetoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjetoId");

                    b.ToTable("AtividadeChannel");
                });

            modelBuilder.Entity("CA.Core.Entidades.Channel.ProjetoChannel", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ProjetoChannel");
                });

            modelBuilder.Entity("CA.Core.Entidades.Channel.UsuarioChannel", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("NomeCompleto")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<string>("NomeUsuario")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("UsuarioChannel");
                });

            modelBuilder.Entity("CA.Identity.Entidades.UsuarioIdentity", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("UsuarioIdentity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<string>", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(450)
                        .HasColumnType("varchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("varchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("varchar(450)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(450)
                        .HasColumnType("varchar(450)");

                    b.HasKey("Id");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450)
                        .HasColumnType("varchar(450)");

                    b.Property<string>("ClaimType")
                        .HasColumnType("varchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("varchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("varchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("varchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UsuarioIdentityClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(450)
                        .HasColumnType("varchar(450)");

                    b.Property<string>("RoleId")
                        .HasMaxLength(450)
                        .HasColumnType("varchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.ToTable("UsuarioIdentityRoles", (string)null);
                });

            modelBuilder.Entity("CA.Core.Entidades.CA.ParametrosIntegracao", b =>
                {
                    b.HasOne("CA.Core.Entidades.CA.UsuarioCA", "Usuario")
                        .WithOne("ParametrosIntegracoes")
                        .HasForeignKey("CA.Core.Entidades.CA.ParametrosIntegracao", "IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("CA.Core.Entidades.CA.UsuarioCA", b =>
                {
                    b.HasOne("CA.Core.Entidades.CA.UsuarioCA", "Gerente")
                        .WithMany()
                        .HasForeignKey("IdGerente")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("CA.Core.Entidades.CA.Unidade", "Unidade")
                        .WithMany()
                        .HasForeignKey("IdUnidade")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Gerente");

                    b.Navigation("Unidade");
                });

            modelBuilder.Entity("CA.Core.Entidades.Channel.ApontamentoChannel", b =>
                {
                    b.HasOne("CA.Core.Entidades.Channel.AtividadeChannel", "Atividade")
                        .WithMany("Apontamentos")
                        .HasForeignKey("AtividadeId");

                    b.HasOne("CA.Core.Entidades.Channel.ProjetoChannel", "Projeto")
                        .WithMany("Apontamentos")
                        .HasForeignKey("ProjetoId");

                    b.HasOne("CA.Core.Entidades.Channel.UsuarioChannel", "Usuario")
                        .WithMany("Apontamentos")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Atividade");

                    b.Navigation("Projeto");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("CA.Core.Entidades.Channel.AtividadeChannel", b =>
                {
                    b.HasOne("CA.Core.Entidades.Channel.ProjetoChannel", "Projeto")
                        .WithMany("Atividades")
                        .HasForeignKey("ProjetoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Projeto");
                });

            modelBuilder.Entity("CA.Identity.Entidades.UsuarioIdentity", b =>
                {
                    b.HasOne("CA.Core.Entidades.CA.UsuarioCA", "Usuario")
                        .WithOne()
                        .HasForeignKey("CA.Identity.Entidades.UsuarioIdentity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CA.Identity.Entidades.UsuarioIdentity", null)
                        .WithMany("UserClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CA.Core.Entidades.CA.UsuarioCA", b =>
                {
                    b.Navigation("ParametrosIntegracoes")
                        .IsRequired();
                });

            modelBuilder.Entity("CA.Core.Entidades.Channel.AtividadeChannel", b =>
                {
                    b.Navigation("Apontamentos");
                });

            modelBuilder.Entity("CA.Core.Entidades.Channel.ProjetoChannel", b =>
                {
                    b.Navigation("Apontamentos");

                    b.Navigation("Atividades");
                });

            modelBuilder.Entity("CA.Core.Entidades.Channel.UsuarioChannel", b =>
                {
                    b.Navigation("Apontamentos");
                });

            modelBuilder.Entity("CA.Identity.Entidades.UsuarioIdentity", b =>
                {
                    b.Navigation("UserClaims");
                });
#pragma warning restore 612, 618
        }
    }
}
