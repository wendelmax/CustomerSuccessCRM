using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Lib.Services.Contracts
{
    public interface IDocumentoService
    {
        // Operações de Contrato
        Task<IEnumerable<Contrato>> GetAllContratosAsync();
        Task<Contrato?> GetContratoByIdAsync(int id);
        Task<Contrato> CreateContratoAsync(Contrato contrato);
        Task<Contrato> UpdateContratoAsync(Contrato contrato);
        Task DeleteContratoAsync(int id);
        Task<Contrato> AprovarContratoAsync(int id, string responsavel);
        Task<Contrato> AssinarContratoAsync(int id, string assinatura);
        Task<byte[]> GerarPDFContratoAsync(int id);
        Task<IEnumerable<Contrato>> GetContratosByClienteAsync(int clienteId);
        Task<IEnumerable<Contrato>> GetContratosVencidosAsync();

        // Operações de Proposta
        Task<IEnumerable<Proposta>> GetAllPropostasAsync();
        Task<Proposta?> GetPropostaByIdAsync(int id);
        Task<Proposta> CreatePropostaAsync(Proposta proposta);
        Task<Proposta> UpdatePropostaAsync(Proposta proposta);
        Task DeletePropostaAsync(int id);
        Task<Proposta> AprovarPropostaAsync(int id, string responsavel);
        Task<Proposta> RejeitarPropostaAsync(int id, string motivo);
        Task<byte[]> GerarPDFPropostaAsync(int id);
        Task<IEnumerable<Proposta>> GetPropostasByClienteAsync(int clienteId);
        Task<IEnumerable<Proposta>> GetPropostasByOportunidadeAsync(int oportunidadeId);
        Task<Contrato> ConverterPropostaEmContratoAsync(int propostaId);

        // Operações de Anexos
        Task<ContratoAnexo> AddContratoAnexoAsync(int contratoId, string nomeArquivo, byte[] conteudo);
        Task<PropostaAnexo> AddPropostaAnexoAsync(int propostaId, string nomeArquivo, byte[] conteudo);
        Task<byte[]> GetAnexoConteudoAsync(string caminhoArquivo);
        Task DeleteAnexoAsync(string caminhoArquivo);
    }
} 