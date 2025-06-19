using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Services;
using CustomerSuccessCRM.Lib.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerSuccessCRM.Lib
{
    /// <summary>
    /// Exemplo de uso da biblioteca CustomerSuccessCRM com SQLite
    /// </summary>
    public static class ExemploUso
    {
        /// <summary>
        /// Exemplo de configuração básica com SQLite
        /// </summary>
        public static void ConfigurarBancoDeDados()
        {
            // Configuração básica do Entity Framework com SQLite
            var optionsBuilder = new DbContextOptionsBuilder<CrmDbContext>();
            optionsBuilder.UseSqlite("Data Source=CustomerSuccessCRM.db");
            
            using var context = new CrmDbContext(optionsBuilder.Options);
            
            // Criar o banco de dados se não existir
            context.Database.EnsureCreated();
            
            Console.WriteLine("Banco de dados SQLite configurado com sucesso!");
        }

        /// <summary>
        /// Exemplo de uso dos serviços
        /// </summary>
        public static async Task ExemploUsoServicos()
        {
            // Configurar o contexto
            var optionsBuilder = new DbContextOptionsBuilder<CrmDbContext>();
            optionsBuilder.UseSqlite("Data Source=CustomerSuccessCRM.db");
            
            using var context = new CrmDbContext(optionsBuilder.Options);
            
            // Criar instâncias dos repositórios
            var clienteRepository = new Repositories.ClienteRepository(context);
            var produtoRepository = new Repositories.ProdutoRepository(context);
            var metaRepository = new Repositories.MetaRepository(context);
            
            // Criar instâncias dos serviços
            var clienteService = new ClienteService(clienteRepository);
            var produtoService = new ProdutoService(produtoRepository);
            var metaService = new MetaService(metaRepository);

            try
            {
                // Exemplo: Criar um cliente
                var cliente = new Cliente
                {
                    Nome = "João Silva",
                    Email = "joao@empresa.com",
                    Telefone = "(11) 99999-9999",
                    Empresa = "Empresa ABC Ltda"
                };

                await clienteService.CadastrarAsync(cliente);
                Console.WriteLine($"Cliente '{cliente.Nome}' criado com sucesso!");

                // Exemplo: Criar um produto
                var produto = new Produto
                {
                    Nome = "Software CRM",
                    Descricao = "Sistema de gestão de relacionamento com clientes",
                    PrecoBase = 299.99m,
                    Categoria = CategoriaProduto.Software,
                    QuantidadeEstoque = 50
                };

                await produtoService.CadastrarAsync(produto);
                Console.WriteLine($"Produto '{produto.Nome}' criado com sucesso!");

                // Exemplo: Criar uma meta
                var meta = new Meta
                {
                    Nome = "Meta de Vendas Q1",
                    Descricao = "Meta de vendas para o primeiro trimestre",
                    Valor = 100000.00m,
                    Progresso = 0,
                    ResponsavelId = "vendedor1",
                    EquipeId = "equipe1",
                    DataInicio = DateTime.Now,
                    DataFim = DateTime.Now.AddMonths(3)
                };

                await metaService.CadastrarAsync(meta);
                Console.WriteLine($"Meta '{meta.Nome}' criada com sucesso!");

                // Exemplo: Buscar todos os clientes
                var clientes = await clienteService.ListarTodosAsync();
                Console.WriteLine($"Total de clientes: {clientes.Count}");

                // Exemplo: Buscar produtos por categoria
                var produtosSoftware = await produtoService.BuscarPorCategoriaAsync(CategoriaProduto.Software);
                Console.WriteLine($"Produtos de software: {produtosSoftware.Count}");

                // Exemplo: Calcular percentual de atingimento das metas
                var percentualAtingimento = await metaService.CalcularPercentualAtingimentoAsync();
                Console.WriteLine($"Percentual de atingimento das metas: {percentualAtingimento:F2}%");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }

        /// <summary>
        /// Exemplo de configuração com injeção de dependência
        /// </summary>
        public static void ExemploConfiguracaoDI()
        {
            // Este exemplo mostra como configurar em uma aplicação ASP.NET Core
            
            /*
            // No Program.cs ou Startup.cs:
            
            var builder = WebApplication.CreateBuilder(args);
            
            // Configuração básica
            builder.Services.AddCustomerSuccessCRMServices();
            
            // Ou com string de conexão personalizada
            builder.Services.AddCustomerSuccessCRMServices("Data Source=MeuCRM.db");
            
            var app = builder.Build();
            
            // No controller:
            public class ClienteController : ControllerBase
            {
                private readonly ClienteService _clienteService;
                
                public ClienteController(ClienteService clienteService)
                {
                    _clienteService = clienteService;
                }
                
                [HttpGet]
                public async Task<IActionResult> GetClientes()
                {
                    var clientes = await _clienteService.ListarTodosAsync();
                    return Ok(clientes);
                }
            }
            */
        }
    }
} 