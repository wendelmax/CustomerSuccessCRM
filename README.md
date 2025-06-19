# CustomerSuccessCRM

Sistema de CRM (Customer Relationship Management) focado em Customer Success, desenvolvido em .NET 8.0.

## 📋 Funcionalidades

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
- IDE: Visual Studio 2022, VS Code ou JetBrains Rider
- Git

### 🐧 Configuração Específica para Linux

#### Instalação do .NET 8.0 SDK

**Ubuntu/Debian:**
```bash
# Adicionar repositório Microsoft
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt update

# Instalar .NET 8.0 SDK
sudo apt install dotnet-sdk-8.0

# Verificar instalação
dotnet --version
```

**Fedora/RHEL:**
```bash
# Adicionar repositório Microsoft
sudo dnf install dotnet-sdk-8.0

# Verificar instalação
dotnet --version
```

**Arch Linux:**
```bash
# Via pacman
sudo pacman -S dotnet-sdk

# Verificar instalação
dotnet --version
```

#### 🛠️ IDEs Recomendadas para Linux

**JetBrains Rider (Recomendado):**
- **Download**: https://www.jetbrains.com/rider/download/
- **Vantagens**: 
  - IntelliSense completo para C#
  - Debugging avançado com breakpoints
  - Refactoring automático
  - Integração nativa com Git
  - Suporte a Docker e containers
  - Performance otimizada para Linux
  - Experiência profissional completa

**Visual Studio Code:**
```bash
# Instalar VS Code
sudo snap install code --classic

# Extensões recomendadas:
# - C# Dev Kit
# - C# Extensions
# - Avalonia for Visual Studio Code
# - .NET Install Tool
```

**Instalação via Snap:**
```bash
sudo snap install rider --classic
```

#### 🎯 Configuração do Projeto no Linux

```bash
# 1. Clone o repositório
git clone https://github.com/seu-usuario/CustomerSuccessCRM.git
cd CustomerSuccessCRM

# 2. Restaurar pacotes
dotnet restore

# 3. Configurar banco de dados
cd CustomerSuccessCRM.Web
dotnet ef database update

# 4. Executar aplicação Web
dotnet run

# 5. Em outro terminal, executar aplicação Desktop
cd CustomerSuccessCRM.Desktop
dotnet run
```

#### 🔧 Configurações Específicas do Linux

**Permissões de execução:**
```bash
# Tornar executável (se necessário)
chmod +x CustomerSuccessCRM.Desktop/bin/Debug/net8.0/CustomerSuccessCRM.Desktop
```

**Dependências do sistema:**
```bash
# Ubuntu/Debian - Dependências para Avalonia UI
sudo apt install libc6-dev libgdiplus libx11-dev

# Fedora/RHEL
sudo dnf install libgdiplus libX11-devel

# Arch Linux
sudo pacman -S gdiplus libx11
```

**Configuração do Rider:**
1. Abrir o projeto no Rider
2. Configurar SDK: `File > Settings > Build, Execution, Deployment > Toolset and Build`
3. Verificar .NET 8.0 SDK está selecionado
4. Configurar run configurations para Web e Desktop

### 🪟 Configuração para Windows

```bash
# Instalar .NET 8.0 SDK via winget
winget install Microsoft.DotNet.SDK.8

# Ou baixar do site oficial
# https://dotnet.microsoft.com/download/dotnet/8.0
```

### 🍎 Configuração para macOS

```bash
# Via Homebrew
brew install dotnet

# Ou baixar do site oficial
# https://dotnet.microsoft.com/download/dotnet/8.0
```

### Instalação Universal

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

### ✅ Implementado

#### Customer Success Manager
- **Gestão de Clientes**: Cadastro, edição e busca de clientes
- **Gestão de Metas**: Definição e acompanhamento de metas de sucesso
- **Dashboard**: Visualização de métricas e progresso em tempo real
- **Relatórios**: Acompanhamento de performance e atingimento

#### Administrador de Produtos
- **Gestão de Produtos**: Cadastro e manutenção do catálogo
- **Controle de Estoque**: Acompanhamento de quantidades
- **Gestão de Preços**: Atualização e controle de preços
- **Categorização**: Organização por categorias (Hardware, Software, Serviço, Treinamento)

#### Analista de Dados
- **Dashboard Interativo**: Métricas de clientes, produtos e metas
- **Relatórios de Progresso**: Acompanhamento de metas em tempo real
- **Análise de Performance**: Percentual de conclusão e status
- **Dados Recentes**: Lista de clientes e atividades recentes

#### Equipe de Vendas
- **Consulta de Clientes**: Busca e visualização de informações
- **Acompanhamento de Metas**: Progresso de objetivos de vendas
- **Gestão de Produtos**: Acesso ao catálogo completo
- **Relatórios**: Métricas de performance e atingimento

### 🚀 Planejado para Futuras Versões

#### Vendedor
- **Gestão de Oportunidades**: Pipeline de vendas completo
- **Acompanhamento de Propostas**: Status e follow-up
- **Controle de Pipeline**: Fases de vendas e conversões
- **Histórico de Clientes**: Interações e oportunidades anteriores

#### Administrador do Sistema
- **Gestão de Usuários**: Controle de acesso e permissões
- **Configurações Avançadas**: Personalização do sistema
- **Backup e Restauração**: Gestão de dados
- **Logs e Auditoria**: Monitoramento de atividades

#### Customer Success Manager (Funcionalidades Avançadas)
- **Gestão de Interações**: Histórico completo de contatos
- **Workflows Automatizados**: Processos de follow-up
- **Notificações**: Alertas e lembretes automáticos
- **Documentos**: Geração de contratos e propostas

#### Integrações
- **Email**: Envio automático de relatórios
- **Calendário**: Agendamento de follow-ups
- **CRM Externo**: Integração com sistemas existentes
- **APIs**: Conectores para ferramentas de terceiros

## 🤝 Contribuindo

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/NovaFuncionalidade`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova funcionalidade'`)
4. Push para a branch (`git push origin feature/NovaFuncionalidade`)
5. Abra um Pull Request

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## 📞 Suporte

Para suporte, abra uma issue no GitHub.

---

**Desenvolvido com ❤️ para otimizar o Customer Success**
