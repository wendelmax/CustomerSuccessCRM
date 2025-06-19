# Customer Success CRM - Desktop

Aplicação desktop do Customer Success CRM desenvolvida com Avalonia UI.

## Características

- **Interface Moderna**: Design limpo e responsivo usando Avalonia UI
- **Multi-plataforma**: Funciona em Windows, Linux e macOS
- **Arquitetura MVVM**: Padrão Model-View-ViewModel para melhor organização
- **Injeção de Dependência**: Uso de Microsoft.Extensions.DependencyInjection
- **Integração com Biblioteca**: Compartilha a mesma biblioteca da aplicação Web

## Funcionalidades Implementadas

### Dashboard
- Cards de estatísticas (Total de Clientes, Produtos, Metas)
- Percentual de conclusão de metas
- Lista de clientes recentes
- Lista de metas pendentes

### Gestão de Clientes
- Listagem com busca em tempo real
- Criação de novos clientes
- Edição de clientes existentes
- Exclusão de clientes
- Filtros por nome, email e empresa

## Estrutura do Projeto

```
CustomerSuccessCRM.Desktop/
├── Views/
│   ├── Dashboard/
│   │   ├── DashboardView.axaml
│   │   └── DashboardView.axaml.cs
│   ├── Clientes/
│   │   ├── ClientesView.axaml
│   │   └── ClientesView.axaml.cs
│   └── MainWindow.axaml
├── ViewModels/
│   ├── Dashboard/
│   │   └── DashboardViewModel.cs
│   ├── Clientes/
│   │   └── ClientesViewModel.cs
│   ├── MainWindowViewModel.cs
│   └── ViewModelBase.cs
├── Program.cs
└── App.axaml.cs
```

## Como Executar

### Pré-requisitos
- .NET 8.0 ou superior
- Avalonia UI

### Execução
```bash
# Navegar para o diretório do projeto
cd CustomerSuccessCRM.Desktop

# Restaurar dependências
dotnet restore

# Executar a aplicação
dotnet run
```

### Compilação
```bash
# Compilar em modo Debug
dotnet build

# Compilar em modo Release
dotnet build --configuration Release
```

## Tecnologias Utilizadas

- **Avalonia UI**: Framework UI multi-plataforma
- **CommunityToolkit.Mvvm**: Biblioteca para MVVM
- **Microsoft.Extensions.DependencyInjection**: Injeção de dependência
- **CustomerSuccessCRM.Lib**: Biblioteca compartilhada com a Web

## Próximos Passos

- [ ] Implementar ViewModels e Views para Produtos
- [ ] Implementar ViewModels e Views para Metas
- [ ] Adicionar validações de formulário
- [ ] Implementar relatórios
- [ ] Adicionar configurações da aplicação
- [ ] Implementar testes unitários
- [ ] Adicionar ícones e recursos visuais
- [ ] Implementar temas claro/escuro

## Contribuição

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](../LICENSE) para mais detalhes. 