using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Services;

namespace CustomerSuccessCRM.Desktop.ViewModels.Metas;

public partial class MetasViewModel : ViewModelBase
{
    private readonly MetaService _metaService;

    [ObservableProperty]
    private ObservableCollection<Meta> _metas = new();

    [ObservableProperty]
    private Meta? _selectedMeta;

    [ObservableProperty]
    private Meta _novaMeta = new();

    [ObservableProperty]
    private string _searchTerm = string.Empty;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private decimal _progresso;

    public MetasViewModel(MetaService metaService)
    {
        _metaService = metaService;
        LoadMetasCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadMetas()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            var metas = await _metaService.ListarTodasAsync();
            Metas.Clear();
            foreach (var meta in metas)
            {
                Metas.Add(meta);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erro ao carregar metas: {ex.Message}";
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

            // Por enquanto, vamos filtrar localmente
            var metas = await _metaService.ListarTodasAsync();
            var metasFiltradas = metas.Where(m => 
                m.Nome.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                m.Descricao?.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) == true
            ).ToList();
            
            Metas.Clear();
            foreach (var meta in metasFiltradas)
            {
                Metas.Add(meta);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erro ao buscar metas: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void StartAdd()
    {
        NovaMeta = new Meta
        {
            DataInicio = DateTime.Now,
            DataFim = DateTime.Now.AddMonths(1),
            Status = StatusMeta.EmAndamento
        };
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

            if (SelectedMeta != null)
            {
                await _metaService.AtualizarAsync(SelectedMeta);
            }
            else
            {
                await _metaService.CadastrarAsync(NovaMeta);
            }

            IsEditing = false;
            await LoadMetas();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erro ao salvar meta: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void StartEdit(Meta meta)
    {
        SelectedMeta = meta;
        Progresso = meta.Progresso;
        IsEditing = true;
        ErrorMessage = string.Empty;
    }

    [RelayCommand]
    private void Cancel()
    {
        IsEditing = false;
        SelectedMeta = null;
        ErrorMessage = string.Empty;
    }

    [RelayCommand]
    private async Task Delete(Meta meta)
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            await _metaService.DeletarAsync(meta.Id);
            await LoadMetas();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erro ao excluir meta: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task AtualizarProgresso()
    {
        if (SelectedMeta == null) return;

        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            await _metaService.AtualizarProgressoAsync(SelectedMeta.Id, Progresso);
            await LoadMetas();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erro ao atualizar progresso: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task ConcluirMeta(Meta meta)
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            await _metaService.AtualizarProgressoAsync(meta.Id, meta.Valor);
            await LoadMetas();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erro ao concluir meta: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
} 