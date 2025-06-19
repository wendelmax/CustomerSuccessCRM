# CustomerSuccessCRM

Sistema de CRM (Customer Relationship Management) focado em Customer Success, desenvolvido em .NET 8.0.

## 📋 Funcionalidades

- Gestão completa de clientes
- Acompanhamento de interações
- Gestão de oportunidades e propostas
- Workflows automatizados
- Notificações e alertas
- Geração de documentos (contratos e propostas)
- Dashboard com métricas de sucesso
- Integração com email
- Armazenamento de arquivos

## 🚀 Tecnologias Utilizadas

- ASP.NET Core 8.0
- Entity Framework Core
- SQLite (desenvolvimento)
- Bootstrap 5
- AdminLTE 3
- Avalonia UI (Desktop)
- xUnit (testes)
- Moq (mocking para testes)

## 📦 Estrutura do Projeto

O projeto está organizado em quatro camadas principais:

1. **CustomerSuccessCRM.Web**: Interface web do sistema
   - Controllers
   - Views
   - Configurações
   - Assets estáticos

2. **CustomerSuccessCRM.Lib**: Biblioteca principal
   - Models
   - Repositories
   - Services
   - Business Rules
   - Configurações

3. **CustomerSuccessCRM.Desktop**: Interface desktop (Avalonia UI)
   - Views
   - ViewModels
   - Componentes multi-plataforma

4. **CustomerSuccessCRM.Tests**: Testes unitários
   - Testes de serviços
   - Mocks
   - Fixtures

## ⚙️ Configuração do Ambiente

1. Pré-requisitos:
   - .NET 8.0 SDK
   - Visual Studio 2022 ou VS Code
   - Git

2. Clone o repositório:
```bash
git clone https://github.com/seu-usuario/CustomerSuccessCRM.git
cd CustomerSuccessCRM
```

3. Restaure os pacotes:
```bash
dotnet restore
```

4. Execute as migrações:
```bash
cd CustomerSuccessCRM.Web
dotnet ef database update
```

5. Execute o projeto Web:
```bash
dotnet run
```

6. Execute o projeto Desktop:
```bash
cd CustomerSuccessCRM.Desktop
dotnet run
```

## 🖥️ Aplicação Desktop

A aplicação desktop foi desenvolvida usando **Avalonia UI**, oferecendo:

- **Multi-plataforma**: Funciona em Windows, Linux e macOS
- **Interface Moderna**: Design limpo e responsivo
- **Arquitetura MVVM**: Padrão Model-View-ViewModel
- **Integração Completa**: Compartilha a mesma biblioteca da Web

### Funcionalidades do Desktop

- **Dashboard**: Visão geral com estatísticas e dados recentes
- **Gestão de Clientes**: CRUD completo com busca e filtros
- **Navegação Intuitiva**: Menu lateral com acesso rápido às funcionalidades

Para mais detalhes, consulte o [README do Desktop](CustomerSuccessCRM.Desktop/README.md).

## 🧪 Executando os Testes

Para executar os testes unitários:

```bash
cd CustomerSuccessCRM.Tests
dotnet test
```

## 🔧 Configurações

O sistema utiliza vários arquivos de configuração:

- **EmailSettings**: Configurações de email
- **StorageSettings**: Configurações de armazenamento
- **NotificationSettings**: Configurações de notificações
- **WorkflowSettings**: Configurações de workflows
- **MetaSettings**: Configurações de metas
- **ProdutoSettings**: Configurações de produtos

## 📚 Documentação dos Serviços

### CrmService
Gerencia as operações principais do CRM:
- Gestão de clientes
- Gestão de interações
- Gestão de oportunidades

### WorkflowService
Gerencia os workflows automatizados:
- Validação de regras
- Execução de ações
- Notificações de status

### StorageService
Gerencia o armazenamento de arquivos:
- Upload de arquivos
- Download de arquivos
- Validação de tipos permitidos
- Controle de tamanho máximo

### EmailService
Gerencia o envio de emails:
- Templates personalizados
- Anexos
- Filas de envio

### NotificationService
Gerencia as notificações do sistema:
- Notificações em tempo real
- Alertas
- Histórico de notificações

## 🤝 Contribuindo

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## 📞 Suporte

Para suporte, envie um email para suporte@customersuccesscrm.com ou abra uma issue no GitHub.
