using CommunityToolkit.Mvvm.ComponentModel;

namespace CustomerSuccessCRM.Desktop.ViewModels;

public abstract class ViewModelBase : ObservableObject
{
    public virtual void OnNavigatedTo()
    {
        // Método chamado quando o ViewModel é ativado
    }

    public virtual void OnNavigatedFrom()
    {
        // Método chamado quando o ViewModel é desativado
    }
}
