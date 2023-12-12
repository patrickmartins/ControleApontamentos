using CA.Core.Entidades.CA;
using CA.Core.Entidades.Channel;
using CA.Identity.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CA.Repositorios.Contexto
{
    public class ContextoDadosCA : DbContext
    {
        public ContextoDadosCA(DbContextOptions<ContextoDadosCA> options) : base(options) { ChangeTracker.AutoDetectChangesEnabled = false; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region CA

            modelBuilder.Entity<UsuarioCA>()
                        .HasKey(c => c.Id);

            modelBuilder.Entity<UsuarioCA>()
                        .Property(c => c.Id)
                        .ValueGeneratedNever();

            modelBuilder.Entity<UsuarioCA>()
                        .Property(c => c.NomeCompleto)
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .IsRequired();

            modelBuilder.Entity<UsuarioCA>()
                        .HasOne(c => c.ParametrosIntegracoes)
                        .WithOne(c => c.Usuario)
                        .HasForeignKey(typeof(ParametrosIntegracao), "IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UsuarioCA>()
                        .HasOne(c => c.Unidade)
                        .WithMany()
                        .HasForeignKey(c => c.IdUnidade)                        
                        .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<UsuarioCA>()
                        .HasOne(c => c.Gerente)
                        .WithMany()
                        .HasForeignKey(c => c.IdGerente)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ParametrosIntegracao>()
                        .HasKey(c => c.IdUsuario);

            modelBuilder.Entity<ParametrosIntegracao>()
                        .Property(c => c.IdUsuarioChannel)
                        .ValueGeneratedNever();

            modelBuilder.Entity<ParametrosIntegracao>()
                        .Property(c => c.IdUsuarioTfs)
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

            modelBuilder.Entity<ParametrosIntegracao>()
                        .Property(c => c.DominioTfs)
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

            modelBuilder.Entity<ParametrosIntegracao>()
                        .Property(c => c.NomeUsuarioTfs)
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

            modelBuilder.Entity<ParametrosIntegracao>()
                        .Property(c => c.TipoIdUsuarioTfs)
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

            modelBuilder.Entity<ParametrosIntegracao>()
                        .Property(c => c.IdUsuarioChannel);

            modelBuilder.Entity<ParametrosIntegracao>()
                        .Property(c => c.NomeUsuarioChannel)
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

            modelBuilder.Entity<ParametrosIntegracao>()
                        .Property(c => c.IdFuncionarioPonto);

            modelBuilder.Entity<ParametrosIntegracao>()
                        .Property(c => c.PisFuncionarioPonto)
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

            modelBuilder.Entity<Unidade>()
                        .HasKey(c => c.Id);

            modelBuilder.Entity<Unidade>()
                        .Property(c => c.Nome)
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

            modelBuilder.Entity<Unidade>()
                        .Property(c => c.Id)
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .ValueGeneratedOnAdd();
            #endregion

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
                        .Property(c => c.IdApontamentoTfs)
                        .IsRequired();

            modelBuilder.Entity<ApontamentoChannel>()
                        .Property(c => c.AtividadeId);

            modelBuilder.Entity<ApontamentoChannel>()
                        .Property(c => c.AtividadeId);

            modelBuilder.Entity<ApontamentoChannel>()
                        .Property(c => c.UsuarioId)
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
                        .WithMany(c => c.Apontamentos)
                        .HasForeignKey(c => c.UsuarioId);

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
                        .WithOne(c => c.Projeto)
                        .HasForeignKey(c => c.ProjetoId);

            modelBuilder.Entity<ProjetoChannel>()
                        .HasMany(c => c.Apontamentos)
                        .WithOne(c => c.Projeto)
                        .HasForeignKey(c => c.ProjetoId);

            modelBuilder.Entity<AtividadeChannel>()
                        .HasKey(c => c.Id);

            modelBuilder.Entity<AtividadeChannel>()
                        .Property(c => c.Id)
                        .ValueGeneratedNever();

            modelBuilder.Entity<AtividadeChannel>()
                        .Property(c => c.ProjetoId);

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
                        .WithOne(c => c.Atividade)
                        .HasForeignKey(c => c.AtividadeId);

            #endregion

            #region Identity

            modelBuilder.Entity<UsuarioIdentity>()
                        .HasKey(c => c.Id);

            modelBuilder.Entity<UsuarioIdentity>()
                        .Property(c => c.Id)
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")                        
                        .ValueGeneratedNever();

            modelBuilder.Entity<UsuarioIdentity>()
                        .Property(c => c.UserName)
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

            modelBuilder.Entity<UsuarioIdentity>()
                        .Property(c => c.NormalizedUserName)
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

            modelBuilder.Entity<UsuarioIdentity>()
                        .Property(c => c.Email)
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

            modelBuilder.Entity<UsuarioIdentity>()
                        .Property(c => c.NormalizedEmail)
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

            modelBuilder.Entity<UsuarioIdentity>()
                        .Property(c => c.EmailConfirmed)
                        .IsRequired();

            modelBuilder.Entity<UsuarioIdentity>()
                        .Property(c => c.PasswordHash)
                        .HasColumnType("nvarchar(max)");

            modelBuilder.Entity<UsuarioIdentity>()
                        .Property(c => c.SecurityStamp)
                        .HasColumnType("nvarchar(max)");

            modelBuilder.Entity<UsuarioIdentity>()
                        .Property(c => c.ConcurrencyStamp)
                        .HasColumnType("nvarchar(max)");

            modelBuilder.Entity<UsuarioIdentity>()
                        .Property(c => c.PhoneNumber)
                        .HasColumnType("nvarchar(max)");

            modelBuilder.Entity<UsuarioIdentity>()
                        .Property(c => c.PhoneNumberConfirmed)
                        .IsRequired();

            modelBuilder.Entity<UsuarioIdentity>()
                        .Property(c => c.TwoFactorEnabled)
                        .IsRequired();

            modelBuilder.Entity<UsuarioIdentity>()
                        .Property(c => c.LockoutEnd);

            modelBuilder.Entity<UsuarioIdentity>()
                        .Property(c => c.LockoutEnabled)
                        .IsRequired();

            modelBuilder.Entity<UsuarioIdentity>()
                        .Property(c => c.AccessFailedCount)
                        .IsRequired();

            modelBuilder.Entity<UsuarioIdentity>()
                        .HasMany(c => c.UserClaims)
                        .WithOne()
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<UsuarioIdentity>()
                        .HasOne(c => c.Usuario)
                        .WithOne()
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasForeignKey(typeof(UsuarioIdentity), "Id");

            modelBuilder.Entity<IdentityRole<string>>()
                        .ToTable("Role")
                        .HasKey(c => c.Id);

            modelBuilder.Entity<IdentityRole<string>>()
                        .Property(c => c.Id)
                        .HasColumnType("varchar(450)")
                        .HasMaxLength(450)
                        .ValueGeneratedNever();

            modelBuilder.Entity<IdentityRole<string>>()
                        .Property(c => c.Name)
                        .HasColumnType("varchar(450)")
                        .HasMaxLength(450)
                        .IsRequired();

            modelBuilder.Entity<IdentityRole<string>>()
                        .Property(c => c.NormalizedName)
                        .HasMaxLength(450)
                        .HasColumnType("varchar(450)");

            modelBuilder.Entity<IdentityRole<string>>()
                        .Property(c => c.ConcurrencyStamp)
                        .HasColumnType("varchar(max)");

            modelBuilder.Entity<IdentityUserClaim<string>>()
                        .ToTable("UsuarioIdentityClaims")
                        .HasKey(c => c.Id);

            modelBuilder.Entity<IdentityUserClaim<string>>()                        
                        .Property(c => c.Id)
                        .ValueGeneratedOnAdd();

            modelBuilder.Entity<IdentityUserClaim<string>>()
                        .Property(c => c.ClaimType)
                        .HasColumnType("varchar(max)");

            modelBuilder.Entity<IdentityUserClaim<string>>()
                        .Property(c => c.ClaimValue)
                        .HasColumnType("varchar(max)");

            modelBuilder.Entity<IdentityUserRole<string>>()
                        .HasKey(c => new { c.UserId, c.RoleId });

            modelBuilder.Entity<IdentityUserRole<string>>()
                        .ToTable("UsuarioIdentityRoles")
                        .Property(c => c.RoleId)
                        .HasMaxLength(450)
                        .HasColumnType("varchar(450)")
                        .ValueGeneratedNever();

            modelBuilder.Entity<IdentityUserRole<string>>()
                        .Property(c => c.UserId)
                        .HasMaxLength(450)
                        .HasColumnType("varchar(450)")
                        .ValueGeneratedNever();

            modelBuilder.Entity<IdentityRoleClaim<string>>()
                        .HasKey(c => c.Id);

            modelBuilder.Entity<IdentityRoleClaim<string>>()
                        .ToTable("RoleClaims")
                        .Property(c => c.Id)
                        .HasMaxLength(450)
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
