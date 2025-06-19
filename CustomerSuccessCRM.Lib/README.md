# CustomerSuccessCRM - Projeto Acadêmico Simplificado

Este é um projeto CRM simplificado desenvolvido para fins acadêmicos, focado em ensinar conceitos básicos de C#, Entity Framework Core e arquitetura em camadas.

## Estrutura Simplificada

### Repositórios
- **IBaseRepository**: Interface base simples sem generics
- **BaseRepository**: Implementação base com métodos CRUD básicos
- **IClienteRepository**: Interface específica para clientes
- **ClienteRepository**: Implementação do repositório de clientes
- **IProdutoRepository**: Interface específica para produtos
- **ProdutoRepository**: Implementação do repositório de produtos
- **IMetaRepository**: Interface específica para metas
- **MetaRepository**: Implementação do repositório de metas

### Serviços
- **ClienteService**: Lógica de negócio para clientes
- **ProdutoService**: Lógica de negócio para produtos
- **MetaService**: Lógica de negócio para metas

### Modelos
- **Cliente**: Modelo simplificado de cliente
- **Produto**: Modelo simplificado de produto
- **Meta**: Modelo simplificado de meta
- **Interacao**: Modelo simplificado de interação
- **Oportunidade**: Modelo simplificado de oportunidade

## Conceitos Demonstrados

1. **Padrão Repository**: Separação entre acesso a dados e lógica de negócio
2. **Injeção de Dependência**: Uso de interfaces para desacoplamento
3. **Entity Framework Core**: ORM para acesso a banco de dados
4. **Async/Await**: Programação assíncrona
5. **Validações**: Validações básicas de entrada
6. **Enums**: Uso de enums para status e tipos

## Funcionalidades Básicas

### Clientes
- Cadastrar, atualizar e deletar clientes
- Buscar por status e vendedor
- Ativar/desativar clientes
- Contar clientes ativos

### Produtos
- Cadastrar, atualizar e deletar produtos
- Buscar por categoria e faixa de preço
- Atualizar preços e estoque
- Listar produtos mais vendidos
- Calcular valor total em estoque

### Metas
- Cadastrar, atualizar e deletar metas
- Atualizar progresso
- Buscar por responsável e equipe
- Listar metas atrasadas
- Calcular percentual de atingimento

## Como Usar

1. Configure a string de conexão no `appsettings.json`
2. Execute as migrações do Entity Framework
3. Registre os serviços no container de DI
4. Use os serviços nos controllers

## Exemplo de Uso

```csharp
// No controller
public class ClientesController : Controller
{
    private readonly ClienteService _clienteService;

    public ClientesController(ClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    public async Task<IActionResult> Index()
    {
        var clientes = await _clienteService.ListarTodosAsync();
        return View(clientes);
    }
}
```

## Configuração do DI

```csharp
// No Program.cs
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IMetaRepository, MetaRepository>();

builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<MetaService>();
```

Este projeto é ideal para estudantes que estão aprendendo C# e querem entender conceitos básicos de arquitetura de software de forma prática e didática. 