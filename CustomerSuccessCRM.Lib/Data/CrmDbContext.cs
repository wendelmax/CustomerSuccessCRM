using Microsoft.EntityFrameworkCore;
using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Lib.Data
{
    public class CrmDbContext : DbContext
    {
        public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Meta> Metas { get; set; }
        public DbSet<Interacao> Interacoes { get; set; }
        public DbSet<Oportunidade> Oportunidades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações básicas dos modelos
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Telefone).HasMaxLength(20);
                entity.Property(e => e.Empresa).HasMaxLength(200);
            });

            modelBuilder.Entity<Produto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descricao).HasMaxLength(500);
                entity.Property(e => e.PrecoBase).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Meta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descricao).HasMaxLength(500);
                entity.Property(e => e.Valor).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Progresso).HasColumnType("decimal(5,2)");
            });

            modelBuilder.Entity<Interacao>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Assunto).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Descricao).HasMaxLength(2000);
                entity.Property(e => e.Responsavel).HasMaxLength(100);
            });

            modelBuilder.Entity<Oportunidade>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Descricao).HasMaxLength(1000);
                entity.Property(e => e.Valor).HasColumnType("decimal(18,2)");
                entity.Property(e => e.VendedorId).HasMaxLength(100);
            });
        }
    }
} 