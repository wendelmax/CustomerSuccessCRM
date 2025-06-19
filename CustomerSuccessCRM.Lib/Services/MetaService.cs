using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerSuccessCRM.Lib.Models;
using Microsoft.Extensions.Options;
using CustomerSuccessCRM.Lib.Configuration;
using CustomerSuccessCRM.Lib.Repositories;

namespace CustomerSuccessCRM.Lib.Services.Implementations
{
    public class MetaService : IMetaService
    {
        private readonly IMetaRepository _repository;
        private readonly MetaSettings _settings;
        private readonly INotificationService _notificationService;

        public MetaService(
            IMetaRepository repository,
            IOptions<MetaSettings> settings,
            INotificationService notificationService)
        {
            _repository = repository;
            _settings = settings.Value;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<Meta>> GetAllMetasAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Meta?> GetMetaByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Meta> CreateMetaAsync(Meta meta)
        {
            return await _repository.AddAsync(meta);
        }

        public async Task<Meta> UpdateMetaAsync(Meta meta)
        {
            return await _repository.UpdateAsync(meta);
        }

        public async Task DeleteMetaAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<Meta> AtualizarProgressoAsync(int id, decimal progresso)
        {
            var meta = await GetMetaByIdAsync(id);
            if (meta == null)
                throw new InvalidOperationException("Meta não encontrada");

            meta.Progresso = progresso;
            if (progresso >= meta.Valor)
            {
                meta.Status = StatusMeta.Concluida;
                await NotificarMetaAtingidaAsync(id);
            }
            return await _repository.UpdateAsync(meta);
        }

        public async Task<Meta> ConcluirMetaAsync(int id)
        {
            var meta = await GetMetaByIdAsync(id);
            if (meta == null)
                throw new InvalidOperationException("Meta não encontrada");

            meta.Status = StatusMeta.Concluida;
            meta.DataConclusao = DateTime.Now;
            await NotificarMetaAtingidaAsync(id);
            return await _repository.UpdateAsync(meta);
        }

        public async Task<Meta> CancelarMetaAsync(int id, string motivo)
        {
            var meta = await GetMetaByIdAsync(id);
            if (meta == null)
                throw new InvalidOperationException("Meta não encontrada");

            meta.Status = StatusMeta.Cancelada;
            meta.Observacoes = motivo;
            return await _repository.UpdateAsync(meta);
        }

        public async Task<IEnumerable<Meta>> GetMetasPorResponsavelAsync(string responsavelId)
        {
            return await _repository.GetMetasByResponsavelAsync(responsavelId);
        }

        public async Task<IEnumerable<Meta>> GetMetasPorEquipeAsync(string equipeId)
        {
            return await _repository.GetMetasByEquipeAsync(equipeId);
        }

        public async Task<IEnumerable<Meta>> GetMetasPorPeriodoAsync(DateTime inicio, DateTime fim)
        {
            return await _repository.GetMetasByPeriodoAsync(inicio, fim);
        }

        public async Task<IEnumerable<Meta>> GetMetasAtrasadasAsync()
        {
            return await _repository.GetMetasAtrasadasAsync();
        }

        public async Task<IEnumerable<Meta>> GetMetasProximasVencerAsync(int dias)
        {
            return await _repository.GetMetasProximasVencerAsync(dias);
        }

        public async Task<decimal> GetPercentualAtingimentoGeralAsync(string? equipeId = null)
        {
            return await _repository.GetPercentualAtingimentoGeralAsync(equipeId);
        }

        public async Task<IEnumerable<MetaHistorico>> GetHistoricoMetaAsync(int metaId)
        {
            return await _repository.GetHistoricoMetaAsync(metaId);
        }

        public async Task<IDictionary<TipoMeta, decimal>> GetAtingimentoPorTipoAsync()
        {
            return await _repository.GetAtingimentoPorTipoAsync();
        }

        public async Task<IDictionary<string, decimal>> GetAtingimentoPorEquipeAsync()
        {
            return await _repository.GetAtingimentoPorEquipeAsync();
        }

        public async Task<IEnumerable<Meta>> GetMetasRecorrentesAsync()
        {
            return await _repository.GetMetasRecorrentesAsync();
        }

        public async Task NotificarMetaAtingidaAsync(int id)
        {
            var meta = await GetMetaByIdAsync(id);
            if (meta == null) return;

            await _notificationService.EnviarNotificacaoAsync(
                meta.ResponsavelId,
                "Meta Atingida",
                $"Parabéns! A meta {meta.Nome} foi atingida.");

            if (!string.IsNullOrEmpty(meta.EquipeId))
            {
                await _notificationService.EnviarNotificacaoAsync(
                    meta.EquipeId,
                    "Meta da Equipe Atingida",
                    $"A meta {meta.Nome} foi atingida pela equipe.");
            }
        }

        public async Task NotificarMetaAtrasadaAsync(int id)
        {
            var meta = await GetMetaByIdAsync(id);
            if (meta == null) return;

            await _notificationService.EnviarNotificacaoAsync(
                meta.ResponsavelId,
                "Meta Atrasada",
                $"Atenção! A meta {meta.Nome} está atrasada.");

            if (!string.IsNullOrEmpty(meta.EquipeId))
            {
                await _notificationService.EnviarNotificacaoAsync(
                    meta.EquipeId,
                    "Meta da Equipe Atrasada",
                    $"A meta {meta.Nome} está atrasada.");
            }
        }

        public async Task NotificarProximaVencerAsync(int id)
        {
            var meta = await GetMetaByIdAsync(id);
            if (meta == null) return;

            await _notificationService.EnviarNotificacaoAsync(
                meta.ResponsavelId,
                "Meta Próxima de Vencer",
                $"Atenção! A meta {meta.Nome} está próxima de vencer.");

            if (!string.IsNullOrEmpty(meta.EquipeId))
            {
                await _notificationService.EnviarNotificacaoAsync(
                    meta.EquipeId,
                    "Meta da Equipe Próxima de Vencer",
                    $"A meta {meta.Nome} está próxima de vencer.");
            }
        }
    }
} 