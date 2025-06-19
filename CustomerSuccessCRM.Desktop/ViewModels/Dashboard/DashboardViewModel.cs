using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CustomerSuccessCRM.Lib.Services;
using CustomerSuccessCRM.Lib.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace CustomerSuccessCRM.Desktop.ViewModels.Dashboard;

public partial class DashboardViewModel : ViewModelBase
{
    private readonly ClienteService _clienteService;
    private readonly ProdutoService _produtoService;
    private readonly MetaService _metaService;

    [ObservableProperty]
    private int _totalClientes;

    [ObservableProperty]
    private int _totalProdutos;

    [ObservableProperty]
    private int _totalMetas;

    [ObservableProperty]
    private int _metasConcluidas;

    [ObservableProperty]
    private double _percentualConclusao;

    [ObservableProperty]
    private ObservableCollection<Cliente> _clientesRecentes;

    [ObservableProperty]
    private ObservableCollection<Meta> _metasPendentes;

    [ObservableProperty]
    private bool _isLoading;

    public DashboardViewModel(
        ClienteService clienteService,
        ProdutoService produtoService,
        MetaService metaService)
    {
        _clienteService = clienteService;
        _produtoService = produtoService;
        _metaService = metaService;

        _clientesRecentes = new ObservableCollection<Cliente>();
        _metasPendentes = new ObservableCollection<Meta>();

        LoadDashboardDataCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadDashboardData()
    {
        IsLoading = true;

        try
        {
            // Carregar estatísticas
            var clientes = await _clienteService.ListarTodosAsync();
            var produtos = await _produtoService.ListarTodosAsync();
            var metas = await _metaService.ListarTodasAsync();

            TotalClientes = clientes.Count;
            TotalProdutos = produtos.Count;
            TotalMetas = metas.Count;
            MetasConcluidas = metas.Count(m => m.Status == StatusMeta.Concluida);
            PercentualConclusao = TotalMetas > 0 ? (double)MetasConcluidas / TotalMetas * 100 : 0;

            // Carregar dados recentes
            var clientesRecentes = clientes.OrderByDescending(c => c.DataCadastro).Take(5);
            var metasPendentes = metas.Where(m => m.Status == StatusMeta.EmAndamento).Take(5);

            ClientesRecentes.Clear();
            MetasPendentes.Clear();

            foreach (var cliente in clientesRecentes)
                ClientesRecentes.Add(cliente);

            foreach (var meta in metasPendentes)
                MetasPendentes.Add(meta);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void RefreshData()
    {
        LoadDashboardDataCommand.Execute(null);
    }
} 