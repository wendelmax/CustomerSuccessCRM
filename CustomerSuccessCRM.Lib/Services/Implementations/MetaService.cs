using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Services.Contracts;
using CustomerSuccessCRM.Lib.Services.Strategies;
using CustomerSuccessCRM.Lib.Configuration;
using Microsoft.Extensions.Options;

namespace CustomerSuccessCRM.Lib.Services.Implementations
{
    public class MetaService : IMetaService
    {
        private readonly IBusinessRuleStrategy _businessRuleStrategy;
        private readonly MetaSettings _metaSettings;
        private readonly INotificationService _notificationService;

        public MetaService(
            IBusinessRuleStrategy businessRuleStrategy,
            IOptions<MetaSettings> metaSettings,
            INotificationService notificationService)
        {
            _businessRuleStrategy = businessRuleStrategy;
            _metaSettings = metaSettings.Value;
            _notificationService = notificationService;
        }

        public async Task<bool> ValidateMetaAsync(Meta meta)
        {
            return await _businessRuleStrategy.ValidateMetaAsync(meta);
        }

        public async Task NotificarProgressoMetaAsync(Meta meta, decimal percentualAtingido)
        {
            if (!_metaSettings.EnableAutomaticNotifications)
                return;

            if (percentualAtingido >= _metaSettings.AlertThresholdPercentage)
            {
                await _notificationService.EnviarNotificacaoAsync(
                    meta.ResponsavelId,
                    $"Meta {meta.Titulo} atingiu {percentualAtingido}% do objetivo!",
                    $"Parabéns! Você está muito próximo de atingir a meta {meta.Titulo}.");
            }
            else if (percentualAtingido <= _metaSettings.WarningThresholdPercentage)
            {
                await _notificationService.EnviarNotificacaoAsync(
                    meta.ResponsavelId,
                    $"Atenção: Meta {meta.Titulo} precisa de atenção",
                    $"A meta {meta.Titulo} está com progresso abaixo do esperado ({percentualAtingido}%).");
            }
        }
    }
} 