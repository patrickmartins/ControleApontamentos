using CA.Core.Entidades.Channel;
using CA.Identity.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CA.Repositorios.Channel.Contexto
{
    public class ContextoDadosCA : DbContext
    {
        public ContextoDadosCA(DbContextOptions<ContextoDadosCA> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Channel

            modelBuilder.Entity<ApontamentoChannel>()
                        .HasKey(c => c.Id);

            modelBuilder.Entity<ApontamentoChannel>()
                        .Property(c => c.Id)
                        .ValueGeneratedNever();

            modelBuilder.Entity<ApontamentoChannel>()
                        .Property(c => c.Data)                        
                        .IsRequired();

            modelBuilder.Entity<ApontamentoChannel>()
                        .HasIndex(c => c.Data);

            modelBuilder.Entity<ApontamentoChannel>()
                        .Property(c => c.IdTarefaTfs);

            modelBuilder.Entity<ApontamentoChannel>()
                        .Property(c => c.Tipo)
                        .IsRequired();

            modelBuilder.Entity<ApontamentoChannel>()
                        .Property(c => c.Status)
                        .IsRequired();

            modelBuilder.Entity<ApontamentoChannel>()
                        .Property(c => c.ApontamentoTfs)
                        .IsRequired();

            modelBuilder.Entity<ApontamentoChannel>()
                        .Property(c => c.Hash)
                        .IsRequired();

            modelBuilder.Entity<ApontamentoChannel>()
                        .Property(c => c.Comentario)
                        .HasColumnType("varchar(5000)")
                        .IsRequired();

            modelBuilder.Entity<ApontamentoChannel>()
                        .Property(c => c.TempoApontado)
                        .IsRequired();

            modelBuilder.Entity<ApontamentoChannel>()
                        .HasOne(c => c.Usuario)
                        .WithMany(c => c.Apontamentos);

            modelBuilder.Entity<UsuarioChannel>()
                        .HasKey(c => c.Id);

            modelBuilder.Entity<UsuarioChannel>()
                        .Property(c => c.Id)
                        .ValueGeneratedNever();

            modelBuilder.Entity<UsuarioChannel>()
                        .Property(c => c.NomeUsuario)
                        .HasColumnType("varchar(50)")
                        .IsRequired();

            modelBuilder.Entity<UsuarioChannel>()
                        .Property(c => c.Email)
                        .HasColumnType("varchar(100)")
                        .IsRequired();

            modelBuilder.Entity<UsuarioChannel>()
                        .Property(c => c.NomeCompleto)
                        .HasColumnType("varchar(200)")
                        .IsRequired();

            modelBuilder.Entity<UsuarioChannel>()
                        .Property(c => c.Ativo)
                        .IsRequired();

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

            modelBuilder.Entity<ProjetoChannel>()
                        .HasMany(c => c.Apontamentos)
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

            #endregion

            #region Identity

            modelBuilder.Entity<Usuario>()
                        .HasKey(c => c.Id);

            modelBuilder.Entity<Usuario>()
                        .Property(c => c.Id)
                        .HasColumnType("varchar(450)")
                        .ValueGeneratedNever();

            modelBuilder.Entity<Usuario>()
                        .Property(c => c.UserName)
                        .HasColumnType("varchar(256)");

            modelBuilder.Entity<Usuario>()
                        .Property(c => c.NormalizedUserName)
                        .HasColumnType("varchar(256)");

            modelBuilder.Entity<Usuario>()
                        .Property(c => c.Email)
                        .HasColumnType("varchar(256)");

            modelBuilder.Entity<Usuario>()
                        .Property(c => c.NormalizedEmail)
                        .HasColumnType("varchar(256)");

            modelBuilder.Entity<Usuario>()
                        .Property(c => c.EmailConfirmed)
                        .IsRequired();

            modelBuilder.Entity<Usuario>()
                        .Property(c => c.PasswordHash)
                        .HasColumnType("varchar(max)");

            modelBuilder.Entity<Usuario>()
                        .Property(c => c.SecurityStamp)
                        .HasColumnType("varchar(max)");

            modelBuilder.Entity<Usuario>()
                        .Property(c => c.ConcurrencyStamp)
                        .HasColumnType("varchar(max)");

            modelBuilder.Entity<Usuario>()
                        .Property(c => c.PhoneNumber)
                        .HasColumnType("varchar(max)");

            modelBuilder.Entity<Usuario>()
                        .Property(c => c.PhoneNumberConfirmed)
                        .IsRequired();

            modelBuilder.Entity<Usuario>()
                        .Property(c => c.TwoFactorEnabled)
                        .IsRequired();

            modelBuilder.Entity<Usuario>()
                        .Property(c => c.LockoutEnd);

            modelBuilder.Entity<Usuario>()
                        .Property(c => c.LockoutEnabled)
                        .IsRequired();

            modelBuilder.Entity<Usuario>()
                        .Property(c => c.AccessFailedCount)
                        .IsRequired();

            modelBuilder.Entity<Usuario>()
                        .HasMany(c => c.Claims)
                        .WithOne()
                        .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Role>()
                        .HasKey(c => c.Id);

            modelBuilder.Entity<Role>()
                        .Property(c => c.Id)
                        .HasColumnType("varchar(450)")
                        .ValueGeneratedNever();

            modelBuilder.Entity<Role>()
                        .Property(c => c.Name)
                        .HasColumnType("varchar(450)");

            modelBuilder.Entity<Role>()
                        .Property(c => c.NormalizedName)
                        .HasColumnType("varchar(450)");

            modelBuilder.Entity<Role>()
                        .Property(c => c.ConcurrencyStamp)
                        .HasColumnType("varchar(max)")
                        .IsRequired();

            modelBuilder.Entity<Role>()
                        .Property(c => c.Name)
                        .HasColumnType("varchar(450)")
                        .IsRequired();

            modelBuilder.Entity<IdentityUserClaim<string>>()
                        .HasKey(c => c.Id);

            modelBuilder.Entity<IdentityUserClaim<string>>()
                        .ToTable("UsuarioClaims")
                        .Property(c => c.Id)
                        .ValueGeneratedOnAdd();

            modelBuilder.Entity<IdentityUserClaim<string>>()
                        .Property(c => c.ClaimType)
                        .HasColumnType("varchar(max)");

            modelBuilder.Entity<IdentityUserClaim<string>>()
                        .Property(c => c.ClaimValue)
                        .HasColumnType("varchar(max)");

            modelBuilder.Entity<IdentityUserRole<string>>()
                        .HasKey(c => new { c.RoleId, c.UserId });

            modelBuilder.Entity<IdentityUserRole<string>>()
                        .ToTable("UsuarioRoles")
                        .Property(c => c.RoleId)
                        .HasColumnType("varchar(450)")
                        .ValueGeneratedNever();

            modelBuilder.Entity<IdentityUserRole<string>>()
                        .Property(c => c.UserId)
                        .HasColumnType("varchar(450)")
                        .ValueGeneratedNever();

            modelBuilder.Entity<IdentityRoleClaim<string>>()
                        .HasKey(c => c.Id);

            modelBuilder.Entity<IdentityRoleClaim<string>>()
                        .ToTable("RoleClaims")
                        .Property(c => c.Id)
                        .HasColumnType("varchar(450)")
                        .ValueGeneratedOnAdd();

            modelBuilder.Entity<IdentityRoleClaim<string>>()
                        .Property(c => c.RoleId)
                        .IsRequired();

            modelBuilder.Entity<IdentityRoleClaim<string>>()
                        .Property(c => c.ClaimType)
                        .HasColumnType("varchar(max)");

            modelBuilder.Entity<IdentityRoleClaim<string>>()
                        .Property(c => c.ClaimValue)
                        .HasColumnType("varchar(max)");

            #endregion 

            base.OnModelCreating(modelBuilder);
        }
    }
}
