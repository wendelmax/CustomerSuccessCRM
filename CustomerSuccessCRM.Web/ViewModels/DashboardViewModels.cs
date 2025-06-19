using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Lib.Models.ViewModels
{
    public class CrmDashboard
    {
        public int TotalClientes { get; set; }
        public int TotalProspectos { get; set; }
        public int TotalClientesVip { get; set; }
        public int TotalInteracoes { get; set; }
        public int InteracoesPendentes { get; set; }
        public int TotalOportunidades { get; set; }
        public int OportunidadesAbertas { get; set; }
        public int OportunidadesFechadas { get; set; }
        public decimal ValorTotalOportunidades { get; set; }
        public decimal ValorTotalOportunidadesFechadas { get; set; }
        public decimal TaxaConversao { get; set; }
        public int TotalProdutos { get; set; }
        public decimal ValorTotalProdutos { get; set; }

        // Dados para gráficos
        public List<ClientePorStatus> ClientesPorStatus { get; set; } = new();
        public List<OportunidadePorFase> OportunidadesPorFase { get; set; } = new();
        public List<InteracaoPorTipo> InteracoesPorTipo { get; set; } = new();
        public List<ProdutoPorCategoria> ProdutosPorCategoria { get; set; } = new();

        // Dados recentes
        public List<Cliente> ClientesRecentes { get; set; } = new();
        public List<Interacao> InteracoesRecentes { get; set; } = new();
        public List<Oportunidade> OportunidadesRecentes { get; set; } = new();
    }

    public class ClientePorStatus
    {
        public StatusCliente Status { get; set; }
        public int Quantidade { get; set; }
        public decimal Percentual { get; set; }
    }

    public class OportunidadePorFase
    {
        public FaseOportunidade Fase { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal Percentual { get; set; }
    }

    public class InteracaoPorTipo
    {
        public TipoInteracao Tipo { get; set; }
        public int Quantidade { get; set; }
        public decimal Percentual { get; set; }
    }

    public class ProdutoPorCategoria
    {
        public CategoriaProduto Categoria { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal Percentual { get; set; }
    }
} 