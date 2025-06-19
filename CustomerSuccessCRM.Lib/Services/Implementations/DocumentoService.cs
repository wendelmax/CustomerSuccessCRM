using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Services.Contracts;
using CustomerSuccessCRM.Lib.Data;
using Microsoft.EntityFrameworkCore;
using MediatR;
using CustomerSuccessCRM.Lib.Services.Events;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace CustomerSuccessCRM.Lib.Services.Implementations
{
    public class DocumentoService : IDocumentoService
    {
        private readonly CrmDbContext _context;
        private readonly IMediator _mediator;
        private readonly IStorageService _storageService;
        private readonly StorageSettings _storageSettings;

        public DocumentoService(
            CrmDbContext context,
            IMediator mediator,
            IStorageService storageService,
            IOptions<StorageSettings> storageSettings)
        {
            _context = context;
            _mediator = mediator;
            _storageService = storageService;
            _storageSettings = storageSettings.Value;
        }

        // Operações de Contrato
        public async Task<IEnumerable<Contrato>> GetAllContratosAsync()
        {
            return await _context.Contratos
                .Include(c => c.Cliente)
                .Include(c => c.Proposta)
                .Include(c => c.Historico)
                .Include(c => c.Anexos)
                .ToListAsync();
        }

        public async Task<Contrato?> GetContratoByIdAsync(int id)
        {
            return await _context.Contratos
                .Include(c => c.Cliente)
                .Include(c => c.Proposta)
                .Include(c => c.Historico)
                .Include(c => c.Anexos)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Contrato> CreateContratoAsync(Contrato contrato)
        {
            contrato.DataCriacao = DateTime.Now;
            contrato.Status = StatusContrato.Rascunho;

            _context.Contratos.Add(contrato);
            await _context.SaveChangesAsync();

            return contrato;
        }

        public async Task<Contrato> UpdateContratoAsync(Contrato contrato)
        {
            var historico = new ContratoHistorico
            {
                ContratoId = contrato.Id,
                DataModificacao = DateTime.Now,
                Modificacao = "Contrato atualizado",
                ResponsavelModificacao = contrato.ResponsavelCriacao ?? "Sistema"
            };

            _context.ContratoHistoricos.Add(historico);
            _context.Contratos.Update(contrato);
            await _context.SaveChangesAsync();

            return contrato;
        }

        public async Task DeleteContratoAsync(int id)
        {
            var contrato = await _context.Contratos.FindAsync(id);
            if (contrato != null)
            {
                _context.Contratos.Remove(contrato);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Contrato> AprovarContratoAsync(int id, string responsavel)
        {
            var contrato = await _context.Contratos.FindAsync(id);
            if (contrato == null)
                throw new InvalidOperationException("Contrato não encontrado");

            if (contrato.Status != StatusContrato.EmAnalise)
                throw new InvalidOperationException("Contrato não está em análise");

            contrato.Status = StatusContrato.Aprovado;
            contrato.ResponsavelAprovacao = responsavel;

            var historico = new ContratoHistorico
            {
                ContratoId = contrato.Id,
                DataModificacao = DateTime.Now,
                Modificacao = "Contrato aprovado",
                ResponsavelModificacao = responsavel
            };

            _context.ContratoHistoricos.Add(historico);
            await _context.SaveChangesAsync();

            await _mediator.Publish(new ContratoAprovadoEvent
            {
                ContratoId = contrato.Id,
                Responsavel = responsavel
            });

            return contrato;
        }

        public async Task<Contrato> AssinarContratoAsync(int id, string assinatura)
        {
            var contrato = await _context.Contratos.FindAsync(id);
            if (contrato == null)
                throw new InvalidOperationException("Contrato não encontrado");

            if (contrato.Status != StatusContrato.Aprovado)
                throw new InvalidOperationException("Contrato não está aprovado");

            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(assinatura));
                contrato.HashAssinatura = Convert.ToBase64String(hashBytes);
            }

            contrato.Status = StatusContrato.Assinado;
            contrato.DataAssinatura = DateTime.Now;

            var historico = new ContratoHistorico
            {
                ContratoId = contrato.Id,
                DataModificacao = DateTime.Now,
                Modificacao = "Contrato assinado",
                ResponsavelModificacao = contrato.ResponsavelAprovacao ?? "Sistema"
            };

            _context.ContratoHistoricos.Add(historico);
            await _context.SaveChangesAsync();

            await _mediator.Publish(new ContratoAssinadoEvent
            {
                ContratoId = contrato.Id,
                HashAssinatura = contrato.HashAssinatura
            });

            return contrato;
        }

        public async Task<byte[]> GerarPDFContratoAsync(int id)
        {
            var contrato = await _context.Contratos
                .Include(c => c.Cliente)
                .Include(c => c.Proposta)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contrato == null)
                throw new InvalidOperationException("Contrato não encontrado");

            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new PdfWriter(memoryStream))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);

                        // Cabeçalho
                        document.Add(new Paragraph($"CONTRATO Nº {contrato.Id}"));
                        document.Add(new Paragraph($"Data: {contrato.DataCriacao:dd/MM/yyyy}"));

                        // Dados do Cliente
                        document.Add(new Paragraph($"Cliente: {contrato.Cliente.NomeCompleto}"));
                        document.Add(new Paragraph($"Email: {contrato.Cliente.Email}"));

                        // Conteúdo
                        document.Add(new Paragraph(contrato.Conteudo));

                        // Valores
                        document.Add(new Paragraph($"Valor Total: R$ {contrato.ValorTotal:N2}"));

                        // Assinaturas
                        if (contrato.Status == StatusContrato.Assinado)
                        {
                            document.Add(new Paragraph($"Assinado em: {contrato.DataAssinatura:dd/MM/yyyy}"));
                            document.Add(new Paragraph($"Hash: {contrato.HashAssinatura}"));
                        }
                    }
                }

                return memoryStream.ToArray();
            }
        }

        public async Task<IEnumerable<Contrato>> GetContratosByClienteAsync(int clienteId)
        {
            return await _context.Contratos
                .Include(c => c.Cliente)
                .Include(c => c.Proposta)
                .Where(c => c.ClienteId == clienteId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Contrato>> GetContratosVencidosAsync()
        {
            var hoje = DateTime.Now.Date;
            return await _context.Contratos
                .Include(c => c.Cliente)
                .Where(c => c.DataVencimento.HasValue && c.DataVencimento.Value < hoje)
                .ToListAsync();
        }

        // Operações de Proposta
        public async Task<IEnumerable<Proposta>> GetAllPropostasAsync()
        {
            return await _context.Propostas
                .Include(p => p.Cliente)
                .Include(p => p.Oportunidade)
                .Include(p => p.Itens)
                .Include(p => p.Historico)
                .Include(p => p.Anexos)
                .ToListAsync();
        }

        public async Task<Proposta?> GetPropostaByIdAsync(int id)
        {
            return await _context.Propostas
                .Include(p => p.Cliente)
                .Include(p => p.Oportunidade)
                .Include(p => p.Itens)
                .Include(p => p.Historico)
                .Include(p => p.Anexos)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Proposta> CreatePropostaAsync(Proposta proposta)
        {
            proposta.DataCriacao = DateTime.Now;
            proposta.Status = StatusProposta.Rascunho;

            _context.Propostas.Add(proposta);
            await _context.SaveChangesAsync();

            return proposta;
        }

        public async Task<Proposta> UpdatePropostaAsync(Proposta proposta)
        {
            var historico = new PropostaHistorico
            {
                PropostaId = proposta.Id,
                DataModificacao = DateTime.Now,
                Modificacao = "Proposta atualizada",
                ResponsavelModificacao = proposta.ResponsavelCriacao ?? "Sistema"
            };

            _context.PropostaHistoricos.Add(historico);
            _context.Propostas.Update(proposta);
            await _context.SaveChangesAsync();

            return proposta;
        }

        public async Task DeletePropostaAsync(int id)
        {
            var proposta = await _context.Propostas.FindAsync(id);
            if (proposta != null)
            {
                _context.Propostas.Remove(proposta);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Proposta> AprovarPropostaAsync(int id, string responsavel)
        {
            var proposta = await _context.Propostas.FindAsync(id);
            if (proposta == null)
                throw new InvalidOperationException("Proposta não encontrada");

            proposta.Status = StatusProposta.Aprovada;
            proposta.ResponsavelAprovacao = responsavel;
            proposta.DataAprovacao = DateTime.Now;

            var historico = new PropostaHistorico
            {
                PropostaId = proposta.Id,
                DataModificacao = DateTime.Now,
                Modificacao = "Proposta aprovada",
                ResponsavelModificacao = responsavel
            };

            _context.PropostaHistoricos.Add(historico);
            await _context.SaveChangesAsync();

            await _mediator.Publish(new PropostaAprovadaEvent
            {
                PropostaId = proposta.Id,
                Responsavel = responsavel
            });

            return proposta;
        }

        public async Task<Proposta> RejeitarPropostaAsync(int id, string motivo)
        {
            var proposta = await _context.Propostas.FindAsync(id);
            if (proposta == null)
                throw new InvalidOperationException("Proposta não encontrada");

            proposta.Status = StatusProposta.Rejeitada;

            var historico = new PropostaHistorico
            {
                PropostaId = proposta.Id,
                DataModificacao = DateTime.Now,
                Modificacao = $"Proposta rejeitada. Motivo: {motivo}",
                ResponsavelModificacao = proposta.ResponsavelCriacao ?? "Sistema"
            };

            _context.PropostaHistoricos.Add(historico);
            await _context.SaveChangesAsync();

            await _mediator.Publish(new PropostaRejeitadaEvent
            {
                PropostaId = proposta.Id,
                Motivo = motivo
            });

            return proposta;
        }

        public async Task<byte[]> GerarPDFPropostaAsync(int id)
        {
            var proposta = await _context.Propostas
                .Include(p => p.Cliente)
                .Include(p => p.Oportunidade)
                .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proposta == null)
                throw new InvalidOperationException("Proposta não encontrada");

            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new PdfWriter(memoryStream))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);

                        // Cabeçalho
                        document.Add(new Paragraph($"PROPOSTA Nº {proposta.Id}"));
                        document.Add(new Paragraph($"Data: {proposta.DataCriacao:dd/MM/yyyy}"));

                        // Dados do Cliente
                        document.Add(new Paragraph($"Cliente: {proposta.Cliente.NomeCompleto}"));
                        document.Add(new Paragraph($"Email: {proposta.Cliente.Email}"));

                        // Itens
                        foreach (var item in proposta.Itens)
                        {
                            document.Add(new Paragraph($"- {item.Produto.Nome}"));
                            document.Add(new Paragraph($"  Quantidade: {item.Quantidade}"));
                            document.Add(new Paragraph($"  Valor Unitário: R$ {item.ValorUnitario:N2}"));
                            document.Add(new Paragraph($"  Desconto: R$ {item.DescontoItem:N2}"));
                        }

                        // Valores
                        document.Add(new Paragraph($"Valor Total: R$ {proposta.ValorTotal:N2}"));
                        document.Add(new Paragraph($"Desconto Total: R$ {proposta.DescontoAplicado:N2}"));

                        // Condições
                        document.Add(new Paragraph(proposta.Conteudo));
                    }
                }

                return memoryStream.ToArray();
            }
        }

        public async Task<IEnumerable<Proposta>> GetPropostasByClienteAsync(int clienteId)
        {
            return await _context.Propostas
                .Include(p => p.Cliente)
                .Include(p => p.Oportunidade)
                .Where(p => p.ClienteId == clienteId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Proposta>> GetPropostasByOportunidadeAsync(int oportunidadeId)
        {
            return await _context.Propostas
                .Include(p => p.Cliente)
                .Include(p => p.Oportunidade)
                .Where(p => p.OportunidadeId == oportunidadeId)
                .ToListAsync();
        }

        public async Task<Contrato> ConverterPropostaEmContratoAsync(int propostaId)
        {
            var proposta = await _context.Propostas
                .Include(p => p.Cliente)
                .Include(p => p.Itens)
                .FirstOrDefaultAsync(p => p.Id == propostaId);

            if (proposta == null)
                throw new InvalidOperationException("Proposta não encontrada");

            if (proposta.Status != StatusProposta.Aprovada)
                throw new InvalidOperationException("Proposta não está aprovada");

            var contrato = new Contrato
            {
                ClienteId = proposta.ClienteId,
                PropostaId = proposta.Id,
                Titulo = $"Contrato - {proposta.Titulo}",
                Conteudo = proposta.Conteudo,
                ValorTotal = proposta.ValorTotal,
                DataCriacao = DateTime.Now,
                Status = StatusContrato.Rascunho,
                ResponsavelCriacao = proposta.ResponsavelAprovacao
            };

            _context.Contratos.Add(contrato);

            proposta.Status = StatusProposta.ConvertidaEmContrato;
            
            await _context.SaveChangesAsync();

            return contrato;
        }

        // Operações de Anexos
        public async Task<ContratoAnexo> AddContratoAnexoAsync(int contratoId, string nomeArquivo, byte[] conteudo)
        {
            var contrato = await _context.Contratos.FindAsync(contratoId);
            if (contrato == null)
                throw new InvalidOperationException("Contrato não encontrado");

            var caminhoArquivo = await _storageService.SaveFileAsync(
                "contratos",
                contratoId.ToString(),
                nomeArquivo,
                conteudo);

            var anexo = new ContratoAnexo
            {
                ContratoId = contratoId,
                NomeArquivo = nomeArquivo,
                CaminhoArquivo = caminhoArquivo,
                DataUpload = DateTime.Now
            };

            _context.ContratoAnexos.Add(anexo);
            await _context.SaveChangesAsync();

            return anexo;
        }

        public async Task<PropostaAnexo> AddPropostaAnexoAsync(int propostaId, string nomeArquivo, byte[] conteudo)
        {
            var proposta = await _context.Propostas.FindAsync(propostaId);
            if (proposta == null)
                throw new InvalidOperationException("Proposta não encontrada");

            var caminhoArquivo = await _storageService.SaveFileAsync(
                "propostas",
                propostaId.ToString(),
                nomeArquivo,
                conteudo);

            var anexo = new PropostaAnexo
            {
                PropostaId = propostaId,
                NomeArquivo = nomeArquivo,
                CaminhoArquivo = caminhoArquivo,
                DataUpload = DateTime.Now
            };

            _context.PropostaAnexos.Add(anexo);
            await _context.SaveChangesAsync();

            return anexo;
        }

        public async Task<byte[]> GetAnexoConteudoAsync(string caminhoArquivo)
        {
            return await _storageService.GetFileAsync(caminhoArquivo);
        }

        public async Task DeleteAnexoAsync(string caminhoArquivo)
        {
            await _storageService.DeleteFileAsync(caminhoArquivo);
        }
    }
} 