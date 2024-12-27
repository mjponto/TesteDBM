# TesteDBM

Aplicação **ASP.NET Core** (versão .NET 8) feito para o teste da empresa DBM - Contact Center que demonstra um CRUD (Create, Read, Update, Delete) da entidade **Produto**, seguindo princípios de **SOLID** e boas práticas de **camadas** (MVC), utilizando:


- **Entity Framework Core** (EF Core) para acesso a dados
- **FluentMigrator** para migrações de banco de dados (SQLite)
- **FluentValidation** para validações
- **xUnit** para testes unitários
- **Docker** para containerização
---
Link do Teste: [Teste CRUD Evernote](https://lite.evernote.com/note/e8bb1b2d-c636-756a-72cd-20f4c9b11a73)

---

## Sumário

1. [Descrição do Projeto](#descrição-do-projeto)
2. [Estrutura do Projeto](#estrutura-do-projeto)
3. [Tecnologias e Padrões de Projeto](#tecnologias-e-padrões-de-projeto)
4. [Desafios e Soluções](#desafios-e-soluções)
5. [Plano de Testes](#plano-de-testes)
6. [Configuração e Execução Local](#configuração-e-execução-local)
7. [Migrações de Banco de Dados](#migrações-de-banco-de-dados)
8. [Executando os Testes Unitários](#executando-os-testes-unitários)
9. [Docker (Build, Pull, Run)](#docker-build-pull-run)

---

## Descrição do Projeto

É uma aplicação completa que implementa operações de CRUD para a entidade **Produto**:

- `Id` (int, chave primária)
- `Nome` (string, obrigatório, máximo 100 caracteres, sem duplicidade)
- `Descricao` (string, opcional)
- `Preco` (decimal, obrigatório, maior que zero)
- `DataCadastro` (DateTime, definido automaticamente)

O projeto possui:

- **API REST** (`ProdutosController`) para operações CRUD via JSON.
- **Camada MVC** (`ProdutosMvcController`) que consome a API via `HttpClient`, fornecendo uma interface simples para cadastro, listagem, edição e exclusão de produtos.
- Banco de dados **SQLite**, criado e mantido via **FluentMigrator**.
- **Testes unitários** com **xUnit**, cobrindo repositórios, serviços e validações.

---

## Estrutura do Projeto

```plaintext
TesteDBM/
├── Controllers/
│   ├── HomeController.cs
│   ├── ProdutosController.cs         # API Controller
│   └── ProdutosMvcController.cs     # MVC Controller (consome a API via HttpClient)
├── Data/
│   ├── AppDbContext.cs              # EF Core DbContext
│   ├── Migrations/
│   │   └── CriarTabelaProdutos.cs   # Migration FluentMigrator
│   └── Repositories/
│       ├── IProdutoRepository.cs
│       └── ProdutoRepository.cs
├── DTOs/
│   ├── ProdutoCreateDto.cs
│   └── ProdutoUpdateDto.cs
├── Models/
│   ├── Produto.cs
│   └── ErrorViewModel.cs
├── Services/
│   ├── IProdutoService.cs
│   └── ProdutoService.cs
├── Validators/
│   └── ProdutoValidator.cs
├── Tests/
│   ├── Repositories/
│   │   └── ProdutoRepositoryTests.cs
│   ├── Services/
│   │   └── ProdutoServiceTests.cs
│   └── Validators/
│       └── ProdutoValidatorTests.cs
├── Program.cs
└── Dockerfile
```

**Responsabilidades**:

- **Controllers**: Recebem requisições HTTP e retornam respostas. Pode ser API (JSON) ou MVC (Views).
- **Data**: Contém o `DbContext` (EF Core) e Repositórios (`Repository Pattern`).
- **DTOs**: Modelos de transferência de dados para inputs/outputs específicos (Create, Update).
- **Models**: Entidades de domínio.
- **Services**: Contêm a lógica de negócio (validação, orquestração de repositórios, regras de negócio).
- **Validators**: Implementações de regras de validação usando FluentValidation.
- **Tests**: Testes unitários (xUnit).

---

## Tecnologias e Padrões de Projeto

1. **ASP.NET Core 8 (C#)**: Framework web para criar aplicações robustas.
2. **MVC + Web API**: Organização em Model-View-Controller e Controllers REST.
3. **SOLID**:
    - SRP: Cada camada/arquivo com responsabilidade única.
    - DIP: Dependência de abstrações (interfaces), não implementações concretas.
4. **Entity Framework Core (SQLite)**: ORM para mapear classes C# para o banco SQLite.
5. **FluentMigrator**: Gera e aplica migrações no banco de dados de forma automatizada.
6. **FluentValidation**: Validações poderosas e flexíveis para nossas entidades/dtos.
7. **xUnit + Moq**: Framework de testes + mock para garantir qualidade do código.
8. **Docker**: Containerização para facilitar deploy e execução em qualquer ambiente.

---

## Desafios e Soluções

- **Validação de duplicidade de nome**: Resolvido usando **FluentValidation** + `NomeExisteAsync` no `Repository`.
- **Separação de camadas**: Implementado **Repository Pattern** e **Service** para respeitar princípios de arquitetura limpa.
- **Migrações**: Escolha do **FluentMigrator** para controle versionado do schema, evitando divergências entre ambientes.
- **Testes com EF In-Memory**: Para o **Repository**, optou-se por usar SQLite in-memory.

---

## Plano de Testes

Os testes unitários foram escritos em **xUnit** e cobrem:

1. **Repositories**:
    - `ProdutoRepositoryTests`
    - Verifica se `AdicionarAsync` salva corretamente, se `RemoverAsync` exclui, etc.

2. **Services**:
    - `ProdutoServiceTests`
    - Garante que as regras de negócio (ex.: validação, duplicidade) estejam funcionando, usando mocks (`Moq`) do repositório.

3. **Validators**:
    - `ProdutoValidatorTests`
    - Validação de nome vazio, preço <= 0, nome duplicado.

Esses testes asseguram que as regras de negócio e persistência funcionem conforme esperado.

---

## Configuração e Execução Local

1. **Instale o .NET 8 (SDK)**
    - [Download .NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
    - Verifique com `dotnet --version` se está instalado.

2. **Clone este repositório**
   ```bash
   git clone https://github.com/mjponto/TesteDBM.git
   cd TesteDBM
   ```

3. **Restaurar dependências**
   ```bash
   dotnet restore
   ```

4. **Compilar**
   ```bash
   dotnet build
   ```

5. **Rodar a aplicação**
   ```bash
   dotnet run --project TesteDBM.csproj
   ```

Por padrão, irá rodar no `https://localhost:7294` e/ou `http://localhost:5295` (conforme launchSettings.json).

Acesse no navegador:

- [Swagger UI](http://localhost:5295/swagger) - Swagger UI
- [Página inicial (MVC)](http://localhost:5295) - Página Inicial (MVC)

---

## Migrações de Banco de Dados

Usamos o **FluentMigrator** para gerenciar a tabela Produtos. As migrações são aplicadas automaticamente na inicialização da aplicação, dentro do `Program.cs`:

```csharp
using (var scope = app.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}
```

Então, ao rodar o `dotnet run`, o SQLite `.db` é criado (ou atualizado) com a tabela Produtos. Não é necessário rodar comando de migração manualmente para uso básico.

Caso queira rodar as migrações manualmente, você poderia:

```bash
dotnet fm migrate --assembly TesteDBM.dll --provider sqlite --connection "Data Source=TesteDBM.db"
```

---

## Executando os Testes Unitários

Para executar os testes (repositórios, serviços, validações), use:

```bash
dotnet test
```

O xUnit coletará e exibirá os resultados no console. Caso queira ver logs de saída, use `--verbosity normal`.

