using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CustomerSuccessCRM.Lib.Services;
using CustomerSuccessCRM.Lib.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace CustomerSuccessCRM.Desktop.ViewModels.Clientes;

public partial class ClientesViewModel : ViewModelBase
{
    private readonly ClienteService _clienteService;

    [ObservableProperty]
    private ObservableCollection<Cliente> _clientes;

    [ObservableProperty]
    private ObservableCollection<Cliente> _clientesFiltrados;

    [ObservableProperty]
    private string _termoBusca = string.Empty;

    [ObservableProperty]
    private Cliente? _clienteSelecionado;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _showCreateDialog;

    [ObservableProperty]
    private bool _showEditDialog;

    [ObservableProperty]
    private bool _showDeleteDialog;

    // Propriedades para o formulário
    [ObservableProperty]
    private string _nome = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _telefone = string.Empty;

    [ObservableProperty]
    private string _empresa = string.Empty;

    [ObservableProperty]
    private StatusCliente _status = StatusCliente.Ativo;

    public ClientesViewModel(ClienteService clienteService)
    {
        _clienteService = clienteService;
        _clientes = new ObservableCollection<Cliente>();
        _clientesFiltrados = new ObservableCollection<Cliente>();

        LoadClientesCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadClientes()
    {
        IsLoading = true;

        try
        {
            var clientes = await _clienteService.ListarTodosAsync();
            
            Clientes.Clear();
            foreach (var cliente in clientes)
                Clientes.Add(cliente);

            AplicarFiltro();
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void AplicarFiltro()
    {
        var filtrados = Clientes.Where(c =>
            string.IsNullOrEmpty(TermoBusca) ||
            c.Nome.Contains(TermoBusca, StringComparison.OrdinalIgnoreCase) ||
            c.Email.Contains(TermoBusca, StringComparison.OrdinalIgnoreCase) ||
            c.Empresa.Contains(TermoBusca, StringComparison.OrdinalIgnoreCase)
        ).ToList();

        ClientesFiltrados.Clear();
        foreach (var cliente in filtrados)
            ClientesFiltrados.Add(cliente);
    }

    [RelayCommand]
    private void ShowCreate()
    {
        LimparFormulario();
        ShowCreateDialog = true;
    }

    [RelayCommand]
    private async Task CreateCliente()
    {
        if (string.IsNullOrWhiteSpace(Nome) || string.IsNullOrWhiteSpace(Email))
            return;

        var cliente = new Cliente
        {
            Nome = Nome,
            Email = Email,
            Telefone = Telefone,
            Empresa = Empresa,
            Status = Status,
            DataCadastro = DateTime.Now
        };

        await _clienteService.CadastrarAsync(cliente);
        
        // Recarregar a lista
        await LoadClientes();
        
        ShowCreateDialog = false;
        LimparFormulario();
    }

    [RelayCommand]
    private void ShowEdit()
    {
        if (ClienteSelecionado == null) return;

        Nome = ClienteSelecionado.Nome;
        Email = ClienteSelecionado.Email;
        Telefone = ClienteSelecionado.Telefone;
        Empresa = ClienteSelecionado.Empresa;
        Status = ClienteSelecionado.Status;

        ShowEditDialog = true;
    }

    [RelayCommand]
    private async Task UpdateCliente()
    {
        if (ClienteSelecionado == null || string.IsNullOrWhiteSpace(Nome) || string.IsNullOrWhiteSpace(Email))
            return;

        ClienteSelecionado.Nome = Nome;
        ClienteSelecionado.Email = Email;
        ClienteSelecionado.Telefone = Telefone;
        ClienteSelecionado.Empresa = Empresa;
        ClienteSelecionado.Status = Status;

        await _clienteService.AtualizarAsync(ClienteSelecionado);
        
        ShowEditDialog = false;
        LimparFormulario();
    }

    [RelayCommand]
    private void ShowDelete()
    {
        if (ClienteSelecionado == null) return;
        ShowDeleteDialog = true;
    }

    [RelayCommand]
    private async Task DeleteCliente()
    {
        if (ClienteSelecionado == null) return;

        await _clienteService.DeletarAsync(ClienteSelecionado.Id);
        
        // Recarregar a lista
        await LoadClientes();
        
        ShowDeleteDialog = false;
        ClienteSelecionado = null;
    }

    [RelayCommand]
    private void CancelDialog()
    {
        ShowCreateDialog = false;
        ShowEditDialog = false;
        ShowDeleteDialog = false;
        LimparFormulario();
    }

    private void LimparFormulario()
    {
        Nome = string.Empty;
        Email = string.Empty;
        Telefone = string.Empty;
        Empresa = string.Empty;
        Status = StatusCliente.Ativo;
    }

    partial void OnTermoBuscaChanged(string value)
    {
        AplicarFiltro();
    }
} 