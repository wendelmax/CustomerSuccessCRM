using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using CustomerSuccessCRM.Lib;
using CustomerSuccessCRM.Lib.Data;
using CustomerSuccessCRM.Desktop.ViewModels;
using System;
using CustomerSuccessCRM.Desktop.ViewModels.Dashboard;
using CustomerSuccessCRM.Desktop.ViewModels.Clientes;
using CustomerSuccessCRM.Desktop.ViewModels.Produtos;
using CustomerSuccessCRM.Desktop.ViewModels.Metas;

namespace CustomerSuccessCRM.Desktop;

class Program
{
    public static IServiceProvider? ServiceProvider { get; private set; }

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        // Configurar serviços
        var services = new ServiceCollection();
            
        // Adicionar serviços do CRM
        services.AddCustomerSuccessCrmServices();

        // Registrar ViewModels
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<ClientesViewModel>();
        services.AddTransient<ProdutosViewModel>();
        services.AddTransient<MetasViewModel>();

        // Construir o provedor de serviços
        ServiceProvider = services.BuildServiceProvider();

        // Garantir que o banco de dados existe
        using (var scope = ServiceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<CrmDbContext>();
            DatabaseConfig.EnsureDatabaseExists(context);
        }

        // Iniciar a aplicação Avalonia
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
