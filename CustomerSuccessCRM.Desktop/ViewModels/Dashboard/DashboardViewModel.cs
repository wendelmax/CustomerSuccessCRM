using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Services;

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
    private decimal _percentualConclusao;

    [ObservableProperty]
    private ObservableCollection<Cliente> _clientesRecentes = new();

    [ObservableProperty]
    private ObservableCollection<MetaViewModel> _metas = new();

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

        LoadDashboardData();
    }

    private async void LoadDashboardData()
    {
        try
        {
            IsLoading = true;

            // Carregar totais
            var clientes = await _clienteService.ListarTodosAsync();
            var produtos = await _produtoService.ListarTodosAsync();
            var metas = await _metaService.ListarTodasAsync();

            TotalClientes = clientes.Count();
            TotalProdutos = produtos.Count();
            TotalMetas = metas.Count();

            // Calcular percentual de conclusão (exemplo: metas concluídas)
            var metasConcluidas = metas.Count(m => m.Status == StatusMeta.Concluida);
            PercentualConclusao = TotalMetas > 0 
                ? Math.Round((decimal)metasConcluidas / TotalMetas * 100, 1)
                : 0;

            // Carregar clientes recentes (últimos 5)
            var clientesRecentes = clientes.OrderByDescending(c => c.DataCadastro).Take(5);
            ClientesRecentes.Clear();
            foreach (var cliente in clientesRecentes)
            {
                ClientesRecentes.Add(cliente);
            }

            // Carregar metas com progresso
            var metasAtivas = metas.Where(m => m.Status != StatusMeta.Concluida).Take(5);
            Metas.Clear();
            foreach (var meta in metasAtivas)
            {
                Metas.Add(new MetaViewModel
                {
                    Id = meta.Id,
                    Descricao = meta.Descricao,
                    PercentualConclusao = CalcularPercentualConclusao(meta)
                });
            }
        }
        catch (Exception ex)
        {
            // TODO: Implementar tratamento de erro adequado
            Console.WriteLine($"Erro ao carregar dados do dashboard: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private decimal CalcularPercentualConclusao(Meta meta)
    {
        if (meta.Valor <= 0) return 0;
        return Math.Round((meta.Progresso / meta.Valor) * 100, 1);
    }
}

public class MetaViewModel
{
    public int Id { get; set; }
    public string Descricao { get; set; } = "";
    public decimal PercentualConclusao { get; set; }
} 