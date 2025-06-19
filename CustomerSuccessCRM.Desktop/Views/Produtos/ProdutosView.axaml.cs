using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CustomerSuccessCRM.Desktop.Views.Produtos;

public partial class ProdutosView : UserControl
{
    public ProdutosView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
} 