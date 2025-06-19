using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CustomerSuccessCRM.Desktop.Views.Metas;

public partial class MetasView : UserControl
{
    public MetasView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
} 