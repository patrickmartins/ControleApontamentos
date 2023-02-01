﻿// <auto-generated />
using System;
using CA.Repositorios.Channel.Contexto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CA.Repositorios.Channel.Migrations
{
    [DbContext(typeof(ContextoDadosChannel))]
    partial class ContextoDadosChannelModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CA.Core.Entidades.Channel.ApontamentoChannel", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("AtividadeId")
                        .HasColumnType("int");

                    b.Property<string>("Comentario")
                        .IsRequired()
                        .HasColumnType("varchar(1000)");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan>("TempoApontado")
                        .HasColumnType("time");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AtividadeId");

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

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("NomeCompleto")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.ToTable("UsuarioChannel");
                });

            modelBuilder.Entity("CA.Core.Entidades.Channel.ApontamentoChannel", b =>
                {
                    b.HasOne("CA.Core.Entidades.Channel.AtividadeChannel", "Atividade")
                        .WithMany("Apontamentos")
                        .HasForeignKey("AtividadeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CA.Core.Entidades.Channel.UsuarioChannel", "Usuario")
                        .WithMany("Apontamentos")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Atividade");

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

            modelBuilder.Entity("CA.Core.Entidades.Channel.AtividadeChannel", b =>
                {
                    b.Navigation("Apontamentos");
                });

            modelBuilder.Entity("CA.Core.Entidades.Channel.ProjetoChannel", b =>
                {
                    b.Navigation("Atividades");
                });

            modelBuilder.Entity("CA.Core.Entidades.Channel.UsuarioChannel", b =>
                {
                    b.Navigation("Apontamentos");
                });
#pragma warning restore 612, 618
        }
    }
}
