using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CustomerSuccessCRM.Lib.Services;
using CustomerSuccessCRM.Desktop.ViewModels.Dashboard;
using CustomerSuccessCRM.Desktop.ViewModels.Clientes;
using CustomerSuccessCRM.Desktop.Views.Dashboard;
using CustomerSuccessCRM.Desktop.Views.Clientes;
using System;

namespace CustomerSuccessCRM.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ClienteService _clienteService;
    private readonly ProdutoService _produtoService;
    private readonly MetaService _metaService;

    [ObservableProperty]
    private string _currentPage = "Dashboard";

    [ObservableProperty]
    private object? _currentView;

    public MainWindowViewModel(
        ClienteService clienteService,
        ProdutoService produtoService,
        MetaService metaService)
    {
        _clienteService = clienteService;
        _produtoService = produtoService;
        _metaService = metaService;

        // Iniciar com o Dashboard
        ShowDashboardCommand.Execute(null);
    }

    [RelayCommand]
    private void ShowDashboard()
    {
        CurrentPage = "Dashboard";
        CurrentView = new DashboardView
        {
            DataContext = new DashboardViewModel(_clienteService, _produtoService, _metaService)
        };
    }

    [RelayCommand]
    private void ShowClientes()
    {
        CurrentPage = "Clientes";
        CurrentView = new ClientesView
        {
            DataContext = new ClientesViewModel(_clienteService)
        };
    }

    [RelayCommand]
    private void ShowProdutos()
    {
        CurrentPage = "Produtos";
        // TODO: Implementar ViewModel e View de Produtos
        CurrentView = null;
    }

    [RelayCommand]
    private void ShowMetas()
    {
        CurrentPage = "Metas";
        // TODO: Implementar ViewModel e View de Metas
        CurrentView = null;
    }
}
