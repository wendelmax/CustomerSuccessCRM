using Microsoft.EntityFrameworkCore;
using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Lib.Data
{
    public class CrmDbContext : DbContext
    {
        public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options)
        {
        }

        // Clientes e Interações
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Interacao> Interacoes { get; set; }

        // Oportunidades e Propostas
        public DbSet<Oportunidade> Oportunidades { get; set; }
        public DbSet<Proposta> Propostas { get; set; }
        public DbSet<PropostaItem> PropostaItens { get; set; }
        public DbSet<PropostaHistorico> PropostaHistoricos { get; set; }
        public DbSet<PropostaAnexo> PropostaAnexos { get; set; }

        // Contratos
        public DbSet<Contrato> Contratos { get; set; }
        public DbSet<ContratoHistorico> ContratoHistoricos { get; set; }
        public DbSet<ContratoAnexo> ContratoAnexos { get; set; }

        // Produtos
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ProdutoVariacao> ProdutoVariacoes { get; set; }
        public DbSet<ProdutoPreco> ProdutoPrecos { get; set; }
        public DbSet<ProdutoBundle> ProdutoBundles { get; set; }

        // Metas
        public DbSet<Meta> Metas { get; set; }
        public DbSet<MetaHistorico> MetaHistoricos { get; set; }

        // Workflows
        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<WorkflowExecucao> WorkflowExecucoes { get; set; }
        public DbSet<WorkflowGatilho> WorkflowGatilhos { get; set; }
        public DbSet<WorkflowCondicao> WorkflowCondicoes { get; set; }
        public DbSet<WorkflowAcao> WorkflowAcoes { get; set; }

        // Usuários e Equipes
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Equipe> Equipes { get; set; }

        // Configurações
        public DbSet<ConfiguracaoEmpresa> ConfiguracoesEmpresa { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações de relacionamentos e regras específicas que não podem ser feitas via Data Annotations
            
            // Interações
            modelBuilder.Entity<Interacao>()
                .HasOne(e => e.Cliente)
                .WithMany(c => c.Interacoes)
                .HasForeignKey(e => e.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Oportunidades
            modelBuilder.Entity<Oportunidade>()
                .HasOne(e => e.Cliente)
                .WithMany(c => c.Oportunidades)
                .HasForeignKey(e => e.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Propostas
            modelBuilder.Entity<Proposta>()
                .HasOne(e => e.Cliente)
                .WithMany()
                .HasForeignKey(e => e.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Proposta>()
                .HasOne(e => e.Oportunidade)
                .WithMany()
                .HasForeignKey(e => e.OportunidadeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PropostaItem>()
                .HasOne(e => e.Proposta)
                .WithMany(p => p.Itens)
                .HasForeignKey(e => e.PropostaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PropostaHistorico>()
                .HasOne(e => e.Proposta)
                .WithMany(p => p.Historico)
                .HasForeignKey(e => e.PropostaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PropostaAnexo>()
                .HasOne(e => e.Proposta)
                .WithMany(p => p.Anexos)
                .HasForeignKey(e => e.PropostaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Contratos
            modelBuilder.Entity<Contrato>()
                .HasOne(e => e.Cliente)
                .WithMany()
                .HasForeignKey(e => e.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Contrato>()
                .HasOne(e => e.Proposta)
                .WithMany()
                .HasForeignKey(e => e.PropostaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ContratoHistorico>()
                .HasOne(e => e.Contrato)
                .WithMany(c => c.Historico)
                .HasForeignKey(e => e.ContratoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ContratoAnexo>()
                .HasOne(e => e.Contrato)
                .WithMany(c => c.Anexos)
                .HasForeignKey(e => e.ContratoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Produtos
            modelBuilder.Entity<ProdutoPreco>()
                .HasOne(e => e.Produto)
                .WithMany(p => p.Precos)
                .HasForeignKey(e => e.ProdutoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProdutoVariacao>()
                .HasOne(e => e.Produto)
                .WithMany(p => p.Variacoes)
                .HasForeignKey(e => e.ProdutoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProdutoBundle>()
                .HasOne(e => e.Produto)
                .WithMany(p => p.Bundles)
                .HasForeignKey(e => e.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Metas
            modelBuilder.Entity<Meta>()
                .HasOne(e => e.Responsavel)
                .WithMany(u => u.MetasResponsavel)
                .HasForeignKey(e => e.ResponsavelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Meta>()
                .HasOne(e => e.Equipe)
                .WithMany(e => e.Metas)
                .HasForeignKey(e => e.EquipeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MetaHistorico>()
                .HasOne(e => e.Meta)
                .WithMany(m => m.Historicos)
                .HasForeignKey(e => e.MetaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Workflows
            modelBuilder.Entity<WorkflowExecucao>()
                .HasOne(e => e.Workflow)
                .WithMany(w => w.Execucoes)
                .HasForeignKey(e => e.WorkflowId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkflowGatilho>()
                .HasOne(e => e.Workflow)
                .WithMany(w => w.Gatilhos)
                .HasForeignKey(e => e.WorkflowId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkflowCondicao>()
                .HasOne(e => e.Workflow)
                .WithMany(w => w.Condicoes)
                .HasForeignKey(e => e.WorkflowId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkflowAcao>()
                .HasOne(e => e.Workflow)
                .WithMany(w => w.Acoes)
                .HasForeignKey(e => e.WorkflowId)
                .OnDelete(DeleteBehavior.Cascade);

            // Equipes
            modelBuilder.Entity<Equipe>()
                .HasOne(e => e.Responsavel)
                .WithMany()
                .HasForeignKey(e => e.ResponsavelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasOne(e => e.Equipe)
                .WithMany(e => e.Membros)
                .HasForeignKey(e => e.EquipeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 