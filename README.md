# CustomerSuccessCRM

Sistema de CRM (Customer Relationship Management) focado em Customer Success, desenvolvido em .NET 8.0.

## �� Funcionalidades

- **Gestão completa de clientes** com histórico de interações
- **Gestão de produtos** com controle de estoque e preços
- **Gestão de metas** com acompanhamento de progresso
- **Dashboard interativo** com métricas em tempo real
- **Acompanhamento de interações** e histórico de contatos
- **Gestão de oportunidades** e propostas
- **Workflows automatizados** para processos de negócio
- **Notificações e alertas** em tempo real
- **Geração de documentos** (contratos e propostas)
- **Integração com email** para comunicação
- **Armazenamento de arquivos** seguro
- **Interface multi-plataforma** (Web e Desktop)

## 🚀 Tecnologias Utilizadas

- **Backend**: ASP.NET Core 8.0, Entity Framework Core, SQLite
- **Frontend Web**: Bootstrap 5, AdminLTE 3, Razor Pages
- **Frontend Desktop**: Avalonia UI (multi-plataforma)
- **Testes**: xUnit, Moq (mocking)
- **Arquitetura**: MVVM, Repository Pattern, Dependency Injection

## 📦 Estrutura do Projeto

O projeto está organizado em quatro camadas principais:

1. **CustomerSuccessCRM.Web**: Interface web do sistema
   - Controllers RESTful
   - Views Razor responsivas
   - Configurações de aplicação
   - Assets estáticos (CSS, JS, imagens)

2. **CustomerSuccessCRM.Lib**: Biblioteca principal compartilhada
   - **Models**: Cliente, Produto, Meta, Interacao, Oportunidade
   - **Repositories**: Acesso a dados com padrão Repository
   - **Services**: Lógica de negócio e operações CRUD
   - **Business Rules**: Validações e regras de negócio
   - **Configurações**: Database, DI, etc.

3. **CustomerSuccessCRM.Desktop**: Interface desktop (Avalonia UI)
   - **Views**: Interfaces de usuário modernas
   - **ViewModels**: Lógica de apresentação (MVVM)
   - **Componentes**: Reutilizáveis e multi-plataforma
   - **Navegação**: Menu lateral intuitivo

4. **CustomerSuccessCRM.Tests**: Testes unitários e de integração
   - Testes de serviços e repositories
   - Mocks e fixtures
   - Cobertura de código

## ⚙️ Configuração do Ambiente

### Pré-requisitos
- .NET 8.0 SDK
- Visual Studio 2022 ou VS Code
- Git

### Instalação

1. **Clone o repositório**:
```bash
git clone https://github.com/seu-usuario/CustomerSuccessCRM.git
cd CustomerSuccessCRM
```

2. **Restaure os pacotes**:
```bash
dotnet restore
```

3. **Configure o banco de dados**:
```bash
cd CustomerSuccessCRM.Web
dotnet ef database update
```

4. **Execute a aplicação Web**:
```bash
dotnet run
```

5. **Execute a aplicação Desktop**:
```bash
cd CustomerSuccessCRM.Desktop
dotnet run
```

## 🖥️ Aplicação Desktop

A aplicação desktop foi desenvolvida usando **Avalonia UI**, oferecendo:

- **Multi-plataforma**: Windows, Linux e macOS
- **Interface Moderna**: Design Material Design com temas
- **Arquitetura MVVM**: Padrão Model-View-ViewModel
- **Integração Completa**: Compartilha a mesma biblioteca da Web

### Funcionalidades do Desktop

#### 📊 Dashboard
- **Cards de estatísticas** com ícones e cores temáticas
- **Gráfico de progresso** das metas em tempo real
- **Lista de clientes recentes** com informações essenciais
- **Métricas de conclusão** e performance

#### 👥 Gestão de Clientes
- **CRUD completo** com validações
- **Busca avançada** por nome, email, telefone
- **Filtros por status** e categoria
- **Histórico de interações** integrado
- **Interface responsiva** com DataGrid

#### 📦 Gestão de Produtos
- **Catálogo completo** com categorias
- **Controle de estoque** e preços
- **Busca e filtros** por categoria e faixa de preço
- **Gestão de vendas** e relatórios
- **Interface intuitiva** para cadastro e edição

#### 🎯 Gestão de Metas
- **Definição de metas** com prazos e valores
- **Acompanhamento de progresso** em tempo real
- **Status automático** (Em Andamento, Concluída, Atrasada)
- **Conclusão rápida** com um clique
- **Relatórios de performance** e atingimento

#### 🎨 Interface e UX
- **Navegação lateral** com ícones intuitivos
- **Diálogos modais** para edição
- **Feedback visual** de loading e erros
- **Design responsivo** e acessível
- **Temas consistentes** em toda a aplicação

## 🧪 Executando os Testes

Para executar os testes unitários:

```bash
cd CustomerSuccessCRM.Tests
dotnet test
```

## 🔧 Configurações

O sistema utiliza configurações modulares:

- **DatabaseConfig**: Configuração do SQLite
- **EmailSettings**: Configurações de email
- **StorageSettings**: Configurações de armazenamento
- **NotificationSettings**: Configurações de notificações
- **WorkflowSettings**: Configurações de workflows

## 📚 Documentação dos Serviços

### ClienteService
Gerencia as operações de clientes:
- Cadastro e atualização de clientes
- Busca e filtros avançados
- Histórico de interações
- Validações de dados

### ProdutoService
Gerencia o catálogo de produtos:
- CRUD de produtos com categorias
- Controle de estoque e preços
- Busca por categoria e faixa de preço
- Relatórios de vendas

### MetaService
Gerencia as metas e objetivos:
- Definição e acompanhamento de metas
- Atualização de progresso
- Status automático baseado em prazos
- Relatórios de performance

### WorkflowService
Gerencia os workflows automatizados:
- Validação de regras de negócio
- Execução de ações automáticas
- Notificações de status
- Integração com processos

### StorageService
Gerencia o armazenamento de arquivos:
- Upload e download seguro
- Validação de tipos permitidos
- Controle de tamanho máximo
- Organização por categorias

### EmailService
Gerencia o envio de emails:
- Templates personalizados
- Anexos e formatação
- Filas de envio
- Confirmações de entrega

### NotificationService
Gerencia as notificações do sistema:
- Notificações em tempo real
- Alertas e lembretes
- Histórico de notificações
- Configurações por usuário

## 🎯 Casos de Uso

### Customer Success Manager
- Acompanhar progresso de clientes
- Definir e monitorar metas de sucesso
- Gerenciar interações e follow-ups
- Gerar relatórios de performance

### Vendedor
- Gerenciar oportunidades
- Acompanhar propostas
- Controlar pipeline de vendas
- Acessar histórico de clientes

### Administrador
- Configurar produtos e preços
- Definir metas organizacionais
- Gerenciar usuários e permissões
- Acompanhar métricas gerais

## 🤝 Contribuindo

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/NovaFuncionalidade`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova funcionalidade'`)
4. Push para a branch (`git push origin feature/NovaFuncionalidade`)
5. Abra um Pull Request

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## 📞 Suporte

Para suporte, envie um email para suporte@customersuccesscrm.com ou abra uma issue no GitHub.

---

**Desenvolvido com ❤️ para otimizar o Customer Success**
