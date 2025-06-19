namespace CustomerSuccessCRM.Lib.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Empresa { get; set; } = string.Empty;
        public string VendedorId { get; set; } = string.Empty;
        public StatusCliente Status { get; set; } = StatusCliente.Prospecto;
        public bool Ativo { get; set; } = true;
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }
    }

    public enum StatusCliente
    {
        Prospecto,
        Ativo,
        Inativo,
        Cancelado
    }
} 