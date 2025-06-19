using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Services;

namespace CustomerSuccessCRM.Desktop.ViewModels.Produtos;

public partial class ProdutosViewModel : ViewModelBase
{
    private readonly ProdutoService _produtoService;

    [ObservableProperty]
    private ObservableCollection<Produto> _produtos = new();

    [ObservableProperty]
    private Produto? _selectedProduto;

    [ObservableProperty]
    private Produto _novoProduto = new();

    [ObservableProperty]
    private string _searchTerm = string.Empty;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public ProdutosViewModel(ProdutoService produtoService)
    {
        _produtoService = produtoService;
        LoadProdutosCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadProdutos()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            var produtos = await _produtoService.ListarTodosAsync();
            Produtos.Clear();
            foreach (var produto in produtos)
            {
                Produtos.Add(produto);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erro ao carregar produtos: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task Search()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            var produtos = await _produtoService.BuscarAsync(SearchTerm);
            Produtos.Clear();
            foreach (var produto in produtos)
            {
                Produtos.Add(produto);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erro ao buscar produtos: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void StartAdd()
    {
        NovoProduto = new Produto();
        IsEditing = true;
        ErrorMessage = string.Empty;
    }

    [RelayCommand]
    private async Task Save()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            if (SelectedProduto != null)
            {
                await _produtoService.AtualizarAsync(SelectedProduto);
            }
            else
            {
                await _produtoService.AdicionarAsync(NovoProduto);
            }

            IsEditing = false;
            await LoadProdutos();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erro ao salvar produto: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void StartEdit(Produto produto)
    {
        SelectedProduto = produto;
        IsEditing = true;
        ErrorMessage = string.Empty;
    }

    [RelayCommand]
    private void Cancel()
    {
        IsEditing = false;
        SelectedProduto = null;
        ErrorMessage = string.Empty;
    }

    [RelayCommand]
    private async Task Delete(Produto produto)
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            await _produtoService.ExcluirAsync(produto.Id);
            await LoadProdutos();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erro ao excluir produto: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
} 