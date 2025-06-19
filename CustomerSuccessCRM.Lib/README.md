# CustomerSuccessCRM.Lib

Biblioteca compartilhada para o sistema CRM de Gestão de Relacionamento com o Cliente.

## Visão Geral

Esta biblioteca contém todas as entidades, repositórios, serviços e configurações necessárias para o funcionamento do sistema CRM. Ela é compartilhada entre os projetos Windows Forms e Web.

## Estrutura do Projeto

### Models
- **Cliente**: Entidade principal para gerenciar informações dos clientes
- **Interacao**: Registra todas as interações com os clientes (chamadas, emails, reuniões, etc.)
- **Oportunidade**: Gerencia oportunidades de vendas e negócios
- **Produto**: Produtos/serviços oferecidos pela empresa
- **CrmDashboard**: Dados para relatórios e métricas do dashboard

### Data
- **CrmDbContext**: Contexto do Entity Framework para gerenciar o banco de dados SQLite

### Repositories
- **IRepository<T>**: Interface base para operações CRUD
- **Repository<T>**: Implementação base do repositório
- **IClienteRepository/ClienteRepository**: Repositório específico para clientes
- **IInteracaoRepository/InteracaoRepository**: Repositório específico para interações
- **IOportunidadeRepository/OportunidadeRepository**: Repositório específico para oportunidades
- **IProdutoRepository/ProdutoRepository**: Repositório específico para produtos

### Services
- **ICrmService/CrmService**: Serviço principal que coordena todas as operações do CRM

### Configuration
- **ServiceCollectionExtensions**: Extensões para configurar injeção de dependências

## Funcionalidades Principais

### Gestão de Clientes
- Cadastro, edição e exclusão de clientes
- Busca por nome, email, empresa, telefone
- Filtros por status (Ativo, Inativo, Prospecto, Cliente VIP)
- Histórico de interações e oportunidades

### Gestão de Interações
- Registro de diferentes tipos de interação (Telefone, Email, Reunião, Visita, Chat)
- Controle de prioridade e status
- Associação a clientes específicos
- Filtros por tipo, status, responsável e período

### Gestão de Oportunidades
- Criação e acompanhamento de oportunidades de vendas
- Controle de fases (Prospecção, Qualificação, Proposta, Negociação, Fechada, Perdida)
- Cálculo de probabilidade de sucesso
- Métricas de conversão e valores

### Gestão de Produtos
- Cadastro de produtos e serviços
- Categorização por tipo
- Controle de preços e status ativo/inativo

### Relatórios e Métricas
- Dashboard com métricas principais
- Total de clientes, interações, oportunidades
- Taxa de conversão de vendas
- Dados para gráficos e análises

## Configuração

### Para Windows Forms
```csharp
// No Program.cs ou onde configurar os serviços
services.AddCrmServices("Data Source=crm.db");
```

### Para Web
```csharp
// No Program.cs
builder.Services.AddCrmServices(builder.Configuration.GetConnectionString("DefaultConnection"));
```

### Para Testes
```csharp
services.AddCrmServicesInMemory();
```

## Dependências

- Microsoft.EntityFrameworkCore (8.0.0)
- Microsoft.EntityFrameworkCore.Sqlite (8.0.0)
- Microsoft.EntityFrameworkCore.Tools (8.0.0)
- Microsoft.EntityFrameworkCore.Design (8.0.0)

## Banco de Dados

O sistema utiliza SQLite como banco de dados principal, oferecendo:
- Facilidade de deploy (arquivo único)
- Não requer instalação de servidor
- Suporte completo a transações ACID
- Compatibilidade com Entity Framework Core

## Dados Iniciais

A biblioteca inclui dados de exemplo para:
- 3 produtos iniciais (CRM Básico, CRM Premium, Consultoria)
- 3 clientes de exemplo com diferentes status
- Configurações de relacionamentos entre entidades

## Próximos Passos

1. Implementar o projeto Windows Forms
2. Implementar o projeto Web
3. Adicionar funcionalidades de exportação de relatórios
4. Implementar sistema de notificações
5. Adicionar autenticação e autorização 