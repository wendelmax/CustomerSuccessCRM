using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Web.ViewModels
{
    public class CrmDashboard
    {
        public int TotalClientes { get; set; }
        public int TotalMetas { get; set; }
        public int TotalInteracoes { get; set; }
        public int TotalProdutos { get; set; }
        
        // Metas
        public int MetasConcluidas { get; set; }
        public int MetasEmAndamento { get; set; }
        public decimal ValorTotalMetas { get; set; }
        
        // Dados para gráficos
        public List<ClientePorStatus> ClientesPorStatus { get; set; } = new();
        public List<ProdutoPorCategoria> ProdutosPorCategoria { get; set; } = new();

        // Dados recentes
        public List<Cliente> ClientesRecentes { get; set; } = new();
        public List<Produto> ProdutosMaisVendidos { get; set; } = new();
    }

    public class ClientePorStatus
    {
        public StatusCliente Status { get; set; }
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