using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CustomerSuccessCRM.Lib.Data;
using CustomerSuccessCRM.Lib.Repositories;
using CustomerSuccessCRM.Lib.Services;

namespace CustomerSuccessCRM.Lib
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomerSuccessCRMServices(this IServiceCollection services, string? connectionString = null)
        {
            // Configuração do banco de dados SQLite
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = "Data Source=CustomerSuccessCRM.db";
            }

            services.AddDbContext<CrmDbContext>(options =>
                options.UseSqlite(connectionString));

            // Registro dos repositórios
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IMetaRepository, MetaRepository>();

            // Registro dos serviços
            services.AddScoped<ClienteService>();
            services.AddScoped<ProdutoService>();
            services.AddScoped<MetaService>();

            return services;
        }
    }
} 