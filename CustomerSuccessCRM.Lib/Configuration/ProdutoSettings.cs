namespace CustomerSuccessCRM.Lib.Configuration
{
    public class ProdutoSettings
    {
        public decimal PercentualDescontoMaximo { get; set; }
        public decimal PercentualDescontoVip { get; set; }
        public int QuantidadeDescontoMinima { get; set; }
        public decimal PercentualDescontoQuantidade { get; set; }
        public string[] AprovadorDescontos { get; set; }
        public bool RequererAprovacaoDesconto { get; set; }
        public decimal LimiteDescontoSemAprovacao { get; set; }
        public bool PermitirEstoqueNegativo { get; set; }
        public int EstoqueMinimoAlerta { get; set; }
    }
} 