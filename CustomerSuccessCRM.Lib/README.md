# CustomerSuccessCRM.Lib

Uma biblioteca simplificada para gerenciamento de CRM (Customer Relationship Management) desenvolvida para fins acadêmicos.

## Características

- **SQLite**: Banco de dados leve e portátil
- **Entity Framework Core**: ORM moderno e eficiente
- **Arquitetura Simples**: Repositórios e serviços básicos
- **Código Limpo**: Adequado para aprendizado

## Estrutura do Projeto

```
CustomerSuccessCRM.Lib/
├── Data/
│   └── CrmDbContext.cs          # Contexto do Entity Framework
├── Models/
│   ├── Cliente.cs               # Modelo de cliente
│   ├── Interacao.cs             # Modelo de interação
│   ├── Meta.cs                  # Modelo de meta
│   ├── Oportunidade.cs          # Modelo de oportunidade
│   └── Produto.cs               # Modelo de produto
├── Repositories/
│   ├── BaseRepository.cs        # Repositório base
│   ├── ClienteRepository.cs     # Repositório de clientes
│   ├── MetaRepository.cs        # Repositório de metas
│   ├── ProdutoRepository.cs     # Repositório de produtos
│   └── Interfaces/              # Interfaces dos repositórios
├── Services/
│   ├── ClienteService.cs        # Serviço de clientes
│   ├── MetaService.cs           # Serviço de metas
│   └── ProdutoService.cs        # Serviço de produtos
└── ServiceCollectionExtensions.cs # Extensões para DI
```

## Instalação

### 1. Adicionar o pacote NuGet

```bash
dotnet add package CustomerSuccessCRM.Lib
```

### 2. Configurar o banco de dados SQLite

```csharp
// Program.cs ou Startup.cs
using CustomerSuccessCRM.Lib;

var builder = WebApplication.CreateBuilder(args);

// Configuração básica com SQLite
builder.Services.AddCustomerSuccessCRMServices();

// Ou com string de conexão personalizada
builder.Services.AddCustomerSuccessCRMServices("Data Source=MeuCRM.db");
```

### 3. Criar e aplicar as migrações

```bash
# Criar a migração inicial
dotnet ef migrations add InitialCreate

# Aplicar a migração ao banco
dotnet ef database update
```

## Uso Básico

### Injeção de Dependência

```csharp
public class ClienteController : ControllerBase
{
    private readonly ClienteService _clienteService;

    public ClienteController(ClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpGet]
    public async Task<IActionResult> GetClientes()
    {
        var clientes = await _clienteService.BuscarTodosAsync();
        return Ok(clientes);
    }
}
```

### Operações CRUD

```csharp
// Criar cliente
var cliente = new Cliente
{
    Nome = "João Silva",
    Email = "joao@email.com",
    Telefone = "(11) 99999-9999",
    Empresa = "Empresa ABC"
};

await _clienteService.AdicionarAsync(cliente);

// Buscar cliente
var clienteEncontrado = await _clienteService.BuscarPorIdAsync(1);

// Atualizar cliente
clienteEncontrado.Telefone = "(11) 88888-8888";
await _clienteService.AtualizarAsync(clienteEncontrado);

// Deletar cliente
await _clienteService.DeletarAsync(1);
```

## Modelos Disponíveis

### Cliente
- **Id**: Identificador único
- **Nome**: Nome do cliente
- **Email**: Email de contato
- **Telefone**: Telefone de contato
- **Empresa**: Nome da empresa
- **DataCadastro**: Data de cadastro
- **DataAtualizacao**: Data da última atualização

### Produto
- **Id**: Identificador único
- **Nome**: Nome do produto
- **Descricao**: Descrição do produto
- **PrecoBase**: Preço base
- **Categoria**: Categoria do produto (Hardware, Software, Serviço, Treinamento)
- **QuantidadeEstoque**: Quantidade em estoque
- **QuantidadeVendida**: Quantidade vendida
- **Ativo**: Status ativo/inativo
- **DataCadastro**: Data de cadastro
- **DataAtualizacao**: Data da última atualização

### Meta
- **Id**: Identificador único
- **Nome**: Nome da meta
- **Descricao**: Descrição da meta
- **Valor**: Valor da meta
- **Progresso**: Progresso atual
- **ResponsavelId**: ID do responsável
- **EquipeId**: ID da equipe
- **Status**: Status da meta (EmAndamento, Concluida, Atrasada, Cancelada)
- **DataInicio**: Data de início
- **DataFim**: Data de fim
- **DataConclusao**: Data de conclusão

### Interacao
- **Id**: Identificador único
- **ClienteId**: ID do cliente
- **Assunto**: Assunto da interação
- **Descricao**: Descrição da interação
- **Tipo**: Tipo da interação (Email, Telefone, Reuniao, Outro)
- **Responsavel**: Responsável pela interação
- **DataInteracao**: Data da interação
- **DataCadastro**: Data de cadastro

### Oportunidade
- **Id**: Identificador único
- **ClienteId**: ID do cliente
- **Titulo**: Título da oportunidade
- **Descricao**: Descrição da oportunidade
- **Valor**: Valor da oportunidade
- **Status**: Status da oportunidade (Prospeccao, Qualificacao, Proposta, Negociacao, Fechada, Perdida)
- **VendedorId**: ID do vendedor
- **DataCriacao**: Data de criação
- **DataFechamento**: Data de fechamento

## Vantagens do SQLite

1. **Portabilidade**: Arquivo único que pode ser copiado facilmente
2. **Simplicidade**: Não requer instalação de servidor
3. **Performance**: Rápido para aplicações pequenas e médias
4. **Zero Configuração**: Funciona imediatamente após a configuração
5. **Ideal para Desenvolvimento**: Perfeito para protótipos e aprendizado

## Banco de Dados

O SQLite cria automaticamente um arquivo `CustomerSuccessCRM.db` no diretório da aplicação. Este arquivo contém todas as tabelas e dados do sistema.

### Localização do Arquivo
- **Desenvolvimento**: Diretório raiz da aplicação
- **Produção**: Diretório configurado na string de conexão

## Contribuição

Este projeto foi desenvolvido para fins acadêmicos e educacionais. Sinta-se à vontade para:

1. Fazer fork do projeto
2. Criar uma branch para sua feature
3. Commit suas mudanças
4. Push para a branch
5. Abrir um Pull Request

## Licença

Este projeto está sob a licença MIT. Veja o arquivo LICENSE para mais detalhes. 