using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Repositories;

namespace CustomerSuccessCRM.Lib.Services
{
    public class MetaService
    {
        private readonly IMetaRepository _repository;

        public MetaService(IMetaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Meta>> ListarTodasAsync()
        {
            return await _repository.BuscarTodasAsync();
        }

        public async Task<Meta> BuscarPorIdAsync(int id)
        {
            return await _repository.BuscarPorIdAsync(id);
        }

        public async Task<bool> CadastrarAsync(Meta meta)
        {
            if (string.IsNullOrEmpty(meta.Nome))
                throw new ArgumentException("Nome da meta é obrigatório");

            if (meta.Valor <= 0)
                throw new ArgumentException("Valor da meta deve ser maior que zero");

            if (meta.DataFim <= meta.DataInicio)
                throw new ArgumentException("Data de fim deve ser posterior à data de início");

            meta.Status = StatusMeta.EmAndamento;
            meta.Progresso = 0;

            return await _repository.AdicionarAsync(meta);
        }

        public async Task<bool> AtualizarAsync(Meta meta)
        {
            var metaExistente = await _repository.BuscarPorIdAsync(meta.Id);
            if (metaExistente == null)
                throw new KeyNotFoundException($"Meta com ID {meta.Id} não encontrada");

            metaExistente.Nome = meta.Nome;
            metaExistente.Descricao = meta.Descricao;
            metaExistente.Valor = meta.Valor;
            metaExistente.Progresso = meta.Progresso;
            metaExistente.Status = meta.Status;

            return await _repository.AtualizarAsync(metaExistente);
        }

        public async Task<bool> DeletarAsync(int id)
        {
            return await _repository.DeletarAsync(id);
        }

        public async Task<bool> AtualizarProgressoAsync(int id, decimal progresso)
        {
            var meta = await _repository.BuscarPorIdAsync(id);
            if (meta == null)
                throw new InvalidOperationException("Meta não encontrada");

            meta.Progresso = progresso;
            if (progresso >= meta.Valor)
            {
                meta.Status = StatusMeta.Concluida;
                meta.DataConclusao = DateTime.Now;
            }

            return await _repository.AtualizarAsync(meta);
        }

        public async Task<List<Meta>> BuscarPorResponsavelAsync(string responsavelId)
        {
            return await _repository.BuscarPorResponsavelAsync(responsavelId);
        }

        public async Task<List<Meta>> BuscarPorEquipeAsync(string equipeId)
        {
            return await _repository.BuscarPorEquipeAsync(equipeId);
        }

        public async Task<List<Meta>> ListarAtrasadasAsync()
        {
            return await _repository.BuscarAtrasadasAsync();
        }

        public async Task<decimal> CalcularPercentualAtingimentoAsync(string? equipeId = null)
        {
            return await _repository.CalcularPercentualAtingimentoAsync(equipeId);
        }
    }
} 