using MediatR;

namespace CustomerSuccessCRM.Lib.Services.Events
{
    public abstract class DomainEvent : INotification
    {
        public DateTime Timestamp { get; private set; }
        public string EventType { get; private set; }
        public string? UserId { get; private set; }

        protected DomainEvent()
        {
            Timestamp = DateTime.Now;
            EventType = GetType().Name;
        }

        public void SetUserId(string userId)
        {
            UserId = userId;
        }
    }

    // Eventos de Contrato
    public class ContratoAprovadoEvent : DomainEvent
    {
        public int ContratoId { get; set; }
        public string Responsavel { get; set; } = string.Empty;
    }

    public class ContratoAssinadoEvent : DomainEvent
    {
        public int ContratoId { get; set; }
        public string HashAssinatura { get; set; } = string.Empty;
    }

    public class ContratoVencidoEvent : DomainEvent
    {
        public int ContratoId { get; set; }
        public DateTime DataVencimento { get; set; }
    }

    // Eventos de Proposta
    public class PropostaAprovadaEvent : DomainEvent
    {
        public int PropostaId { get; set; }
        public string Responsavel { get; set; } = string.Empty;
    }

    public class PropostaRejeitadaEvent : DomainEvent
    {
        public int PropostaId { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }

    public class PropostaConvertidaEmContratoEvent : DomainEvent
    {
        public int PropostaId { get; set; }
        public int ContratoId { get; set; }
    }

    // Eventos de Meta
    public class MetaAtingidaEvent : DomainEvent
    {
        public int MetaId { get; set; }
        public decimal ValorAtingido { get; set; }
        public decimal ValorMeta { get; set; }
    }

    public class MetaEmRiscoEvent : DomainEvent
    {
        public int MetaId { get; set; }
        public decimal ValorAtual { get; set; }
        public decimal ValorMeta { get; set; }
        public int PorcentagemAtingida { get; set; }
    }

    // Eventos de Workflow
    public class WorkflowIniciadoEvent : DomainEvent
    {
        public int WorkflowId { get; set; }
        public string Tipo { get; set; } = string.Empty;
    }

    public class WorkflowConcluidoEvent : DomainEvent
    {
        public int WorkflowId { get; set; }
        public string Resultado { get; set; } = string.Empty;
    }

    public class WorkflowFalhadoEvent : DomainEvent
    {
        public int WorkflowId { get; set; }
        public string Erro { get; set; } = string.Empty;
    }

    // Eventos de Produto
    public class ProdutoEstoqueBaixoEvent : DomainEvent
    {
        public int ProdutoId { get; set; }
        public int QuantidadeAtual { get; set; }
        public int EstoqueMinimo { get; set; }
    }

    public class ProdutoPrecoAtualizadoEvent : DomainEvent
    {
        public int ProdutoId { get; set; }
        public decimal PrecoAntigo { get; set; }
        public decimal PrecoNovo { get; set; }
    }

    // Eventos de Notificação
    public class NotificacaoEnviadaEvent : DomainEvent
    {
        public string NotificacaoId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
    }

    public class NotificacaoLidaEvent : DomainEvent
    {
        public string NotificacaoId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }

    // Eventos de Email
    public class EmailEnviadoEvent : DomainEvent
    {
        public string Para { get; set; } = string.Empty;
        public string Assunto { get; set; } = string.Empty;
        public bool Sucesso { get; set; }
    }

    public class EmailFalhadoEvent : DomainEvent
    {
        public string Para { get; set; } = string.Empty;
        public string Assunto { get; set; } = string.Empty;
        public string Erro { get; set; } = string.Empty;
    }

    // Eventos de Documento
    public class DocumentoGeradoEvent : DomainEvent
    {
        public string Tipo { get; set; } = string.Empty;
        public string NomeArquivo { get; set; } = string.Empty;
        public long Tamanho { get; set; }
    }

    public class DocumentoArmazenadoEvent : DomainEvent
    {
        public string CaminhoArquivo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public long Tamanho { get; set; }
    }
} 