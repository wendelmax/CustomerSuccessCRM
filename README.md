# CustomerSuccessCRM

Sistema de CRM (Customer Relationship Management) focado em Customer Success, modernizado para **.NET 10**.

## 🎨 Modernização UI/UX

O sistema passou por uma revitalização completa de interface, focada em uma estética premium e profissional:

- **Tipografia**: Utilização da fonte 'Inter' em todas as interfaces.
- **Ícones**: Substituição de emojis por ícones vetoriais modernos (Material Symbols Rounded na Web e StreamGeometry no Desktop).
- **Estética**: Implementação de glassmorphism, gradientes sutis e bordas arredondadas.
- **Web**: Integração profunda com DataTables.js para gestão avançada de dados.
- **Desktop**: Interface Avalonia UI refinada com Fluent Theme e ícones consistentes.

## 📋 Funcionalidades

- **Gestão completa de clientes** com histórico de interações e visual moderno.
- **Gestão de produtos** com catálogo revitalizado e controle de estoque.
- **Gestão de metas** com dashboards de progresso em tempo real.
- **Interface multi-plataforma** (Web e Desktop) com design unificado.
- **Arquitetura Robusta**: Implementação do **Generic Repository Pattern** na camada de dados.

## 🚀 Tecnologias Utilizadas

- **Backend**: **.NET 10**, Entity Framework Core 10, SQLite.
- **Web**: ASP.NET Core 10, Razor Pages, Bootstrap 5, DataTables.
- **Desktop**: Avalonia UI 11.3 (Fluent Theme).
- **Arquitetura**: MVVM, Generic Repository Pattern, Dependency Injection.

## 📦 Estrutura do Projeto

1. **CustomerSuccessCRM.Web**: Interface web moderna e responsiva.
2. **CustomerSuccessCRM.Lib**: Biblioteca central com Generic Repository e lógica de negócio.
3. **CustomerSuccessCRM.Desktop**: Aplicação desktop com Avalonia UI e Compiled Bindings.
4. **CustomerSuccessCRM.Tests**: Suíte de testes em xUnit para garantir a estabilidade.

## ⚙️ Configuração do Ambiente

### Pré-requisitos
- **.NET 10 SDK** (Obrigatório)
- Visual Studio 2022 (v17.12+) ou VS Code com C# Dev Kit.

### Instalação e Execução

1. **Clone e Restaure**:
```bash
git clone https://github.com/seu-usuario/CustomerSuccessCRM.git
cd CustomerSuccessCRM
dotnet restore
```

2. **Banco de Dados**:
```bash
cd CustomerSuccessCRM.Web
dotnet ef database update
```

3. **Execução**:
- **Web**: `dotnet run --project CustomerSuccessCRM.Web`
- **Desktop**: `dotnet run --project CustomerSuccessCRM.Desktop`

## 🧪 Executando os Testes

```bash
dotnet test
```

---

**Desenvolvido com ❤️ para otimizar o Customer Success no ecossistema .NET 10**
