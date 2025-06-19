namespace CustomerSuccessCRM.Lib.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal PrecoBase { get; set; }
        public CategoriaProduto Categoria { get; set; }
        public int QuantidadeEstoque { get; set; }
        public int QuantidadeVendida { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }
    }

    public enum CategoriaProduto
    {
        Hardware,
        Software,
        Servico,
        Treinamento
    }
} 