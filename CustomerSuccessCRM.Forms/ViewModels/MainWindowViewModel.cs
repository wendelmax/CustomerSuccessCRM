using ReactiveUI;
using System.Collections.ObjectModel;
using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Services;
using CustomerSuccessCRM.Lib.Repositories;
using CustomerSuccessCRM.Lib.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerSuccessCRM.Forms.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ClienteService _clienteService;
        private readonly ProdutoService _produtoService;
        private readonly MetaService _metaService;

        private ObservableCollection<Cliente> _clientes;
        private ObservableCollection<Produto> _produtos;
        private ObservableCollection<Meta> _metas;

        public MainWindowViewModel()
        {
            // Configurar o contexto do banco
            var optionsBuilder = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<CrmDbContext>();
            optionsBuilder.UseSqlite("Data Source=CustomerSuccessCRM.db");
            var context = new CrmDbContext(optionsBuilder.Options);
            context.Database.EnsureCreated();

            // Configurar os repositórios
            var clienteRepository = new ClienteRepository(context);
            var produtoRepository = new ProdutoRepository(context);
            var metaRepository = new MetaRepository(context);

            // Configurar os serviços
            _clienteService = new ClienteService(clienteRepository);
            _produtoService = new ProdutoService(produtoRepository);
            _metaService = new MetaService(metaRepository);

            _clientes = new ObservableCollection<Cliente>();
            _produtos = new ObservableCollection<Produto>();
            _metas = new ObservableCollection<Meta>();

            CarregarDados();
        }

        public ObservableCollection<Cliente> Clientes
        {
            get => _clientes;
            set => this.RaiseAndSetIfChanged(ref _clientes, value);
        }

        public ObservableCollection<Produto> Produtos
        {
            get => _produtos;
            set => this.RaiseAndSetIfChanged(ref _produtos, value);
        }

        public ObservableCollection<Meta> Metas
        {
            get => _metas;
            set => this.RaiseAndSetIfChanged(ref _metas, value);
        }

        private async void CarregarDados()
        {
            try
            {
                var clientes = await _clienteService.ListarTodosAsync();
                var produtos = await _produtoService.ListarTodosAsync();
                var metas = await _metaService.ListarTodasAsync();

                Clientes.Clear();
                foreach (var cliente in clientes)
                {
                    Clientes.Add(cliente);
                }

                Produtos.Clear();
                foreach (var produto in produtos)
                {
                    Produtos.Add(produto);
                }

                Metas.Clear();
                foreach (var meta in metas)
                {
                    Metas.Add(meta);
                }
            }
            catch (Exception ex)
            {
                // Em uma aplicação real, você trataria o erro adequadamente
                System.Diagnostics.Debug.WriteLine($"Erro ao carregar dados: {ex.Message}");
            }
        }
    }
} 