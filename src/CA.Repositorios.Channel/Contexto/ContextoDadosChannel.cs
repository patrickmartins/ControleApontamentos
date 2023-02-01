using CA.Core.Entidades.Channel;
using Microsoft.EntityFrameworkCore;

namespace CA.Repositorios.Channel.Contexto
{
    public class ContextoDadosChannel : DbContext
    {
        public ContextoDadosChannel(DbContextOptions<ContextoDadosChannel> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioChannel>()
                        .HasKey(c => c.Id);

            modelBuilder.Entity<UsuarioChannel>()
                        .Property(c => c.Id)
                        .ValueGeneratedNever();

            modelBuilder.Entity<UsuarioChannel>()
                        .Property(c => c.Email)
                        .HasColumnType("varchar(100)")
                        .IsRequired();

            modelBuilder.Entity<UsuarioChannel>()
                        .Property(c => c.NomeCompleto)
                        .HasColumnType("varchar(200)")
                        .IsRequired();

            modelBuilder.Entity<ApontamentoChannel>()
                        .HasKey(c => c.Id);

            modelBuilder.Entity<ApontamentoChannel>()
                        .Property(c => c.Id)
                        .ValueGeneratedNever();

            modelBuilder.Entity<ApontamentoChannel>()
                        .Property(c => c.Data)
                        .IsRequired();

            modelBuilder.Entity<ApontamentoChannel>()
                        .Property(c => c.Comentario)
                        .HasColumnType("varchar(1000)")
                        .IsRequired();

            modelBuilder.Entity<ApontamentoChannel>()
                        .Property(c => c.TempoApontado)
                        .IsRequired();

            modelBuilder.Entity<ApontamentoChannel>()
                        .HasOne(c => c.Usuario)
                        .WithMany(c => c.Apontamentos);

            modelBuilder.Entity<ProjetoChannel>()
                        .HasKey(c => c.Id);

            modelBuilder.Entity<ProjetoChannel>()
                        .Property(c => c.Id)
                        .ValueGeneratedNever();

            modelBuilder.Entity<ProjetoChannel>()
                        .Property(c => c.Nome)
                        .HasColumnType("varchar(200)")
                        .IsRequired();

            modelBuilder.Entity<ProjetoChannel>()
                        .Property(c => c.Status)
                        .IsRequired();

            modelBuilder.Entity<ProjetoChannel>()
                        .HasMany(c => c.Atividades)
                        .WithOne(c => c.Projeto);

            modelBuilder.Entity<AtividadeChannel>()
                        .HasKey(c => c.Id);

            modelBuilder.Entity<AtividadeChannel>()
                        .Property(c => c.Id)
                        .ValueGeneratedNever();

            modelBuilder.Entity<AtividadeChannel>()
                        .Property(c => c.Nome)
                        .HasColumnType("varchar(200)")
                        .IsRequired();

            modelBuilder.Entity<AtividadeChannel>()
                        .Property(c => c.Codigo)
                        .HasColumnType("varchar(30)")
                        .IsRequired();

            modelBuilder.Entity<AtividadeChannel>()
                        .HasMany(c => c.Apontamentos)
                        .WithOne(c => c.Atividade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
