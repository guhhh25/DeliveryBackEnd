using ContaCerta.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace ContaCerta.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole<Guid>>().ToTable("Roles");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

            // Configurações das novas entidades
            builder.Entity<Empresa>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<Empresa>()
                      .WithMany()
                      .HasForeignKey(e => e.EmpresaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Produto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Empresa)
                      .WithMany(e => e.Produtos)
                      .HasForeignKey(e => e.EmpresaId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Categoria)
                      .WithMany()
                      .HasForeignKey(e => e.CategoriaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
