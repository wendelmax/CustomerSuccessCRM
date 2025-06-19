using Microsoft.Extensions.DependencyInjection;
using CustomerSuccessCRM.Lib.Data;
using CustomerSuccessCRM.Lib.Repositories;
using CustomerSuccessCRM.Lib.Services;
using CustomerSuccessCRM.Lib.Services.Contracts;
using CustomerSuccessCRM.Lib.Services.Implementations;
using CustomerSuccessCRM.Lib.Services.Strategies;
using Microsoft.Extensions.Configuration;

namespace CustomerSuccessCRM.Lib.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCrmServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configurações
            services.Configure<StorageSettings>(configuration.GetSection("Storage"));
            services.Configure<EmailSettings>(configuration.GetSection("Email"));
            services.Configure<NotificationSettings>(configuration.GetSection("Notification"));
            services.Configure<WorkflowSettings>(configuration.GetSection("Workflow"));
            services.Configure<MetaSettings>(configuration.GetSection("Meta"));
            services.Configure<ProdutoSettings>(configuration.GetSection("Produto"));

            // MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

            // Serviços
            services.AddScoped<ICrmService, CrmService>();
            services.AddScoped<IDocumentoService, DocumentoService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IMetaService, MetaService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IPdfService, PdfService>();
            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<IWorkflowService, WorkflowService>();

            // Repositórios
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IInteracaoRepository, InteracaoRepository>();
            services.AddScoped<IOportunidadeRepository, OportunidadeRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();

            // Estratégias
            services.AddScoped<IBusinessRuleStrategy, BusinessRuleStrategy>();

            return services;
        }

        public static IServiceCollection AddCrmServicesInMemory(this IServiceCollection services)
        {
            // Configurar Entity Framework com banco em memória para testes
            services.AddDbContext<CrmDbContext>(options =>
                options.UseInMemoryDatabase("CrmTestDb"));

            // Registrar repositórios
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IInteracaoRepository, InteracaoRepository>();
            services.AddScoped<IOportunidadeRepository, OportunidadeRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();

            // Registrar serviços
            services.AddScoped<ICrmService, CrmService>();

            return services;
        }
    }
} 