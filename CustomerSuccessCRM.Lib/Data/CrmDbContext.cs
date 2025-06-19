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
        public DbSet<Interacao> Interacoes { get; set; }
        public DbSet<Oportunidade> Oportunidades { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ConfiguracaoEmpresa> ConfiguracoesEmpresa { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Sobrenome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Telefone).HasMaxLength(20);
                entity.Property(e => e.Empresa).HasMaxLength(200);
                entity.Property(e => e.Cargo).HasMaxLength(100);
                entity.Property(e => e.Endereco).HasMaxLength(500);
                entity.Property(e => e.Cidade).HasMaxLength(100);
                entity.Property(e => e.Estado).HasMaxLength(50);
                entity.Property(e => e.CEP).HasMaxLength(20);
                entity.Property(e => e.Observacoes).HasMaxLength(1000);
            });

            // Configuração da entidade Interacao
            modelBuilder.Entity<Interacao>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Assunto).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Descricao).IsRequired().HasMaxLength(2000);
                entity.Property(e => e.Responsavel).HasMaxLength(100);
                entity.Property(e => e.Observacoes).HasMaxLength(500);
                
                entity.HasOne(e => e.Cliente)
                    .WithMany(c => c.Interacoes)
                    .HasForeignKey(e => e.ClienteId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuração da entidade Oportunidade
            modelBuilder.Entity<Oportunidade>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Descricao).HasMaxLength(1000);
                entity.Property(e => e.ValorEstimado).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ValorReal).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Responsavel).HasMaxLength(100);
                entity.Property(e => e.Observacoes).HasMaxLength(500);
                
                entity.HasOne(e => e.Cliente)
                    .WithMany(c => c.Oportunidades)
                    .HasForeignKey(e => e.ClienteId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuração da entidade Produto
            modelBuilder.Entity<Produto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Descricao).HasMaxLength(1000);
                entity.Property(e => e.Preco).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Codigo).HasMaxLength(50);
                entity.Property(e => e.Observacoes).HasMaxLength(500);
            });

            // Configuração da entidade ConfiguracaoEmpresa
            modelBuilder.Entity<ConfiguracaoEmpresa>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RazaoSocial).IsRequired().HasMaxLength(200);
                entity.Property(e => e.NomeFantasia).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CNPJ).IsRequired().HasMaxLength(18);
                entity.HasIndex(e => e.CNPJ).IsUnique();
                entity.Property(e => e.InscricaoEstadual).HasMaxLength(20);
                entity.Property(e => e.Endereco).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Cidade).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Estado).IsRequired().HasMaxLength(2);
                entity.Property(e => e.CEP).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Telefone).IsRequired().HasMaxLength(20);
                entity.Property(e => e.EmailPrincipal).IsRequired().HasMaxLength(150);
                entity.Property(e => e.EmailFinanceiro).HasMaxLength(150);
                entity.Property(e => e.EmailSuporte).HasMaxLength(150);
                entity.Property(e => e.Website).HasMaxLength(200);
                entity.Property(e => e.LogoUrl).HasMaxLength(500);
                entity.Property(e => e.CorPrimaria).HasMaxLength(7);
                entity.Property(e => e.CorSecundaria).HasMaxLength(7);
            });

            // Dados iniciais
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Produtos iniciais
            modelBuilder.Entity<Produto>().HasData(
                new Produto
                {
                    Id = 1,
                    Nome = "Sistema CRM Básico",
                    Descricao = "Sistema CRM com funcionalidades básicas de gestão de clientes",
                    Preco = 299.99m,
                    Codigo = "CRM-BASIC",
                    Categoria = CategoriaProduto.Software,
                    Ativo = true,
                    DataCadastro = DateTime.Now
                },
                new Produto
                {
                    Id = 2,
                    Nome = "Sistema CRM Premium",
                    Descricao = "Sistema CRM completo com todas as funcionalidades avançadas",
                    Preco = 599.99m,
                    Codigo = "CRM-PREMIUM",
                    Categoria = CategoriaProduto.Software,
                    Ativo = true,
                    DataCadastro = DateTime.Now
                },
                new Produto
                {
                    Id = 3,
                    Nome = "Consultoria de Implementação",
                    Descricao = "Serviço de consultoria para implementação do sistema CRM",
                    Preco = 150.00m,
                    Codigo = "CONS-IMP",
                    Categoria = CategoriaProduto.Consultoria,
                    Ativo = true,
                    DataCadastro = DateTime.Now
                }
            );

            // Clientes iniciais
            modelBuilder.Entity<Cliente>().HasData(
                new Cliente
                {
                    Id = 1,
                    Nome = "João",
                    Sobrenome = "Silva",
                    Email = "joao.silva@empresa.com",
                    Telefone = "(11) 99999-9999",
                    Empresa = "Empresa ABC Ltda",
                    Cargo = "Gerente de Vendas",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    Status = StatusCliente.Ativo,
                    DataCadastro = DateTime.Now
                },
                new Cliente
                {
                    Id = 2,
                    Nome = "Maria",
                    Sobrenome = "Santos",
                    Email = "maria.santos@techcorp.com",
                    Telefone = "(21) 88888-8888",
                    Empresa = "TechCorp",
                    Cargo = "Diretora Comercial",
                    Cidade = "Rio de Janeiro",
                    Estado = "RJ",
                    Status = StatusCliente.ClienteVip,
                    DataCadastro = DateTime.Now
                },
                new Cliente
                {
                    Id = 3,
                    Nome = "Pedro",
                    Sobrenome = "Oliveira",
                    Email = "pedro.oliveira@startup.com",
                    Telefone = "(31) 77777-7777",
                    Empresa = "StartupXYZ",
                    Cargo = "CEO",
                    Cidade = "Belo Horizonte",
                    Estado = "MG",
                    Status = StatusCliente.Prospecto,
                    DataCadastro = DateTime.Now
                }
            );

            // Configuração inicial da empresa
            modelBuilder.Entity<ConfiguracaoEmpresa>().HasData(
                new ConfiguracaoEmpresa
                {
                    Id = 1,
                    RazaoSocial = "Customer Success CRM Ltda",
                    NomeFantasia = "CS CRM",
                    CNPJ = "00.000.000/0001-00",
                    Endereco = "Rua Principal, 1000",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    CEP = "01000-000",
                    Telefone = "(11) 3000-0000",
                    EmailPrincipal = "contato@cscrm.com",
                    Website = "https://www.cscrm.com",
                    DataCadastro = DateTime.Now,
                    Ativo = true
                }
            );
        }
    }
} 