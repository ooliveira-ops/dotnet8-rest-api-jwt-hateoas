# REST API with ASP.NET Core 8

[![CI/CD](https://github.com/ooliveira-ops/dotnet8-rest-api-jwt-hateoas/actions/workflows/ci.yml/badge.svg?branch=dev)](https://github.com/ooliveira-ops/dotnet8-rest-api-jwt-hateoas/actions/workflows/ci.yml)

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12-239120?style=flat-square&logo=csharp)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?style=flat-square&logo=docker)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?style=flat-square&logo=microsoftsqlserver)
![License](https://img.shields.io/badge/License-MIT-yellow?style=flat-square)

Projeto desenvolvido durante o curso **"Rest API's From 0 to Azure with ASP.NET Core 8 and Docker"**, conforme descrito na própria documentação Swagger da API.

API RESTful com autenticação JWT (com refresh token), HATEOAS, versionamento de rotas, paginação, upload/download de arquivos, negociação de conteúdo JSON/XML, documentação com Swagger e execução em Docker Compose com SQL Server.

> ℹ️ Esta é a **versão legada** do estudo, em .NET 8. A versão mais recente, em .NET 10, está em [dotnet10-rest-api-jwt-hateoas](https://github.com/ooliveira-ops/dotnet10-rest-api-jwt-hateoas).

---

## 📋 Índice

- [Tecnologias](#-tecnologias)
- [Funcionalidades](#-funcionalidades)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Pré-requisitos](#-pré-requisitos)
- [Como Rodar](#-como-rodar)
- [Variáveis de Ambiente](#-variáveis-de-ambiente)
- [Endpoints](#-endpoints)
- [Licença](#-licença)

---

## 🛠 Tecnologias

- **[.NET 8](https://dotnet.microsoft.com/)** — Framework principal (`net8.0`)
- **C#** — Linguagem de programação
- **SQL Server 2022** — Banco de dados relacional (container `mcr.microsoft.com/mssql/server:2022-latest`)
- **Entity Framework Core 8** (`Microsoft.EntityFrameworkCore.SqlServer`) — ORM
- **Docker & Docker Compose** — Containerização
- **JWT (JSON Web Token)** (`Microsoft.AspNetCore.Authentication.JwtBearer`) — Autenticação e autorização
- **Asp.Versioning.Mvc** — Versionamento de API
- **Swagger** (`Swashbuckle.AspNetCore`) — Documentação da API
- **Serilog** (`Serilog.AspNetCore`, `Serilog.Sinks.Console`) — Logging
- **Evolve** — Migrations de banco de dados via scripts SQL
- **HATEOAS** — Implementação própria (`Hypermedia/`), com enrichers e filtro de hypermedia
- **GitHub Actions** — Pipeline de CI/CD (build da imagem e push para o Docker Hub)

---

## ✨ Funcionalidades

- ✅ CRUD completo de **Pessoas** e **Livros**
- ✅ Autenticação com **JWT**, **refresh token** e revogação de token
- ✅ **HATEOAS** — respostas enriquecidas com links de navegação (`PersonEnricher`, `BookEnricher`)
- ✅ **Versionamento de API** (rotas `api/[controller]/v{version}`)
- ✅ **Paginação, ordenação e filtro por nome** na listagem de pessoas
- ✅ Soft delete de pessoa (`PATCH` habilita/desabilita o registro)
- ✅ Upload (individual e múltiplo) e download de arquivos — **PDF, JPG, JPEG e PNG**
- ✅ Negociação de conteúdo (**JSON** e **XML**), com `406 Not Acceptable` para formatos não suportados
- ✅ Documentação interativa com **Swagger** (redirect da raiz `/` para `/swagger`)
- ✅ **CORS** liberado por política padrão
- ✅ Migrations automáticas com **Evolve** (em ambiente de desenvolvimento)
- ✅ Containerização com **Docker Compose** (API + SQL Server com volume persistente)
- ✅ Pipeline **CI/CD** com GitHub Actions (build e push da imagem para o Docker Hub)

---

## 📁 Estrutura do Projeto

```
Projeto-Curso/
├── .github/
│   └── workflows/
│       └── ci.yml                        # Pipeline CI/CD (build + push Docker Hub)
├── RestWithASPNETUdemy/
│   ├── RestWithASPNETUdemy.slnx          # Solution
│   └── RestWithASPNETUdemy/
│       ├── Business/                     # Interfaces de negócio
│       │   └── Implementations/          # Regras de negócio (Person, Book, Login, File)
│       ├── Configurations/               # TokenConfiguration (JWT)
│       ├── Controllers/                  # AuthController, PersonController,
│       │                                 # BookController, FileController
│       ├── Data/
│       │   ├── Converter/Contract/       # Contratos de conversão (IParser, IBook)
│       │   │   └── Implementations/      # PersonConverter, BookConverter
│       │   └── VO/                       # Value Objects (Person, Book, User,
│       │                                 # Token, RefreshToken, FileDetail)
│       ├── Hypermedia/                   # Implementação HATEOAS
│       │   ├── Abstract/                 # IResponseEnricher, ISupportsHypermedia
│       │   ├── Constants/                # Verbos HTTP, tipos de relação e formatos
│       │   ├── Enricher/                 # PersonEnricher, BookEnricher
│       │   ├── Filters/                  # HyperMediaFilter e suas opções
│       │   └── Utils/                    # PagedSearchVO (paginação)
│       ├── Model/
│       │   ├── Base/                     # BaseEntity
│       │   ├── Context/                  # SQLServerContext (EF Core)
│       │   ├── Person.cs / Book.cs / User.cs
│       ├── Properties/
│       │   └── launchSettings.json       # Perfis de execução local
│       ├── Repository/
│       │   ├── Generic/                  # IRepository<T> e GenericRepository<T>
│       │   ├── Implementations/          # BookRepositoryImplementation
│       │   ├── PersonRepository.cs
│       │   └── UserRepository.cs
│       ├── Services/                     # ITokenService
│       │   └── Implementations/          # TokenService (geração/validação de JWT)
│       ├── UploadDir/                    # Diretório de uploads
│       ├── db/
│       │   ├── migrations/               # Scripts DDL (person, book, users, enabled)
│       │   └── dataset/                  # Scripts DML (dados iniciais)
│       ├── Program.cs                    # Ponto de entrada e configuração da aplicação
│       └── RestWithASPNETUdemy.csproj
├── .env.example                          # Modelo das variáveis de ambiente
├── Dockerfile                            # Imagem da aplicação (SDK 8 → ASP.NET 8)
├── docker-compose.yml                    # Orquestração (API + SQL Server)
└── LICENSE
```

> ⚠️ Os diretórios `db/migrations/` e `db/dataset/` estão listados no `.gitignore`, mas os scripts foram versionados antes dessa regra — eles continuam no repositório. Novos scripts precisam ser adicionados com `git add -f`.

---

## ✅ Pré-requisitos

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (para o modo com containers)
- [.NET 8 SDK](https://dotnet.microsoft.com/download) (para rodar sem Docker)
- [Git](https://git-scm.com/)
- Instância do SQL Server acessível (o `docker-compose.yml` já provisiona uma)

---

## 🚀 Como Rodar

### Com Docker (recomendado)

```bash
# Clone o repositório
git clone https://github.com/ooliveira-ops/dotnet8-rest-api-jwt-hateoas.git
cd dotnet8-rest-api-jwt-hateoas

# Copie o .env.example para .env e preencha com seus dados reais
cp .env.example .env

# Suba os containers
docker compose up --build
```

- API: `http://localhost:5000` (a raiz redireciona para `/swagger`)
- SQL Server: `localhost:1433`

Os dados do banco ficam no volume `sqlserver_data`, então persistem entre reinicializações dos containers.

> ⚠️ O container da API sobe com `ASPNETCORE_ENVIRONMENT=Production` e, nesse ambiente, o Evolve **não** executa as migrations automaticamente (elas só rodam quando o ambiente é `Development`). Na primeira subida, crie o banco e rode os scripts de [`db/migrations/`](RestWithASPNETUdemy/RestWithASPNETUdemy/db/migrations) e [`db/dataset/`](RestWithASPNETUdemy/RestWithASPNETUdemy/db/dataset) manualmente, ou rode a aplicação uma vez em `Development` apontando para o mesmo banco.

### Localmente (sem Docker)

```bash
# Restaure as dependências
dotnet restore RestWithASPNETUdemy/RestWithASPNETUdemy/RestWithASPNETUdemy.csproj

# Crie o appsettings.json (veja a seção Variáveis de Ambiente)

# Rode a aplicação
dotnet run --project RestWithASPNETUdemy/RestWithASPNETUdemy
```

Perfis definidos em `Properties/launchSettings.json`:

- **http** — `http://localhost:5055`
- **https** — `https://localhost:7036` e `http://localhost:5055`

Rodando com `ASPNETCORE_ENVIRONMENT=Development`, as migrations do Evolve são aplicadas automaticamente na inicialização.

---

## 🔐 Variáveis de Ambiente

O projeto usa um arquivo `.env` na raiz (baseado no [`.env.example`](.env.example)), referenciado pelo `docker-compose.yml`:

```env
DB_NAME=nome-do-banco-aqui
DB_PASSWORD=sua-senha-forte-aqui
DB_HOST=localhost,1433
DB_USER=sa
JWT_SECRET=gere-uma-chave-aleatoria-aqui
```

O `docker-compose.yml` consome diretamente **`DB_NAME`** e **`DB_PASSWORD`** — esta última alimenta tanto `MSSQL_SA_PASSWORD` (container do SQL Server) quanto a connection string injetada na API via `SQLServerConnection__Connection`. As demais servem para execução local e como referência para o `appsettings.json`.

> 🔑 `JWT_SECRET` é a chave usada para assinar os tokens. Gere um valor aleatório próprio (ex.: `openssl rand -base64 32`) — nunca reaproveite exemplos ou valores de outros projetos.

> 🔐 O SQL Server exige senha forte para o usuário `sa`: mínimo de 8 caracteres, com maiúscula, minúscula, número e símbolo.

> ⚠️ O `.env` nunca é commitado (está no `.gitignore`). Copie o `.env.example` e preencha com seus valores reais.

### appsettings.json

O `appsettings.json` **não é versionado** (está no `.gitignore`). Crie-o em `RestWithASPNETUdemy/RestWithASPNETUdemy/` com a estrutura lida pelo `Program.cs`:

```json
{
  "SQLServerConnection": {
    "Connection": "Server=localhost,1433;Database=<database>;User Id=<user>;Password=<password>;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "TokenConfiguration": {
    "Audience": "ExampleAudience",
    "Issuer": "ExampleIssuer",
    "Secret": "sua-chave-secreta-aqui",
    "Minutes": 60,
    "DaysToExpiry": 7
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

Alternativamente, use o **User Secrets** do .NET (o projeto já tem um `UserSecretsId` configurado):

```bash
dotnet user-secrets set "SQLServerConnection:Connection" "<sua-connection-string>" --project RestWithASPNETUdemy/RestWithASPNETUdemy
dotnet user-secrets set "TokenConfiguration:Secret" "<sua-chave>" --project RestWithASPNETUdemy/RestWithASPNETUdemy
```

> ℹ️ Rodando via Docker, a seção `TokenConfiguration` **não** é injetada pelo `docker-compose.yml`. Para autenticação funcionar em container, adicione as variáveis correspondentes ao serviço `api` (ex.: `TokenConfiguration__Secret=${JWT_SECRET}`, além de `Issuer`, `Audience`, `Minutes` e `DaysToExpiry`).

### Secrets do GitHub Actions

O pipeline em [`.github/workflows/ci.yml`](.github/workflows/ci.yml) faz login no Docker Hub e publica a imagem. Configure em `Settings → Secrets and variables → Actions`:

- `DOCKER_USERNAME`
- `DOCKER_PASSWORD`

> ⚠️ use sempre `.env`, User Secrets ou secrets do GitHub Actions.

---

## 📡 Endpoints

Todas as rotas seguem o padrão `api/[controller]/v{version}` — atualmente na versão `v1`. Exceto os endpoints de login e refresh, todas exigem o header `Authorization: Bearer <accessToken>`.

### Autenticação

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/auth/v1/signin` | Login — retorna `accessToken` e `refreshToken` |
| POST | `/api/auth/v1/refresh` | Renova o token a partir do par access/refresh |
| GET | `/api/auth/v1/revoke` | Revoga o refresh token do usuário autenticado |

### Pessoas

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/person/v1/{sortDirection}/{pageSize}/{page}` | Lista paginada (filtro opcional por `?name=`) |
| GET | `/api/person/v1/{id}` | Busca por ID |
| GET | `/api/person/v1/findPersonByName?firstName=&lastName=` | Busca por nome |
| POST | `/api/person/v1` | Cria uma pessoa |
| PUT | `/api/person/v1` | Atualiza uma pessoa |
| PATCH | `/api/person/v1/{id}` | Desabilita a pessoa (soft delete) |
| DELETE | `/api/person/v1/{id}` | Remove uma pessoa |

### Livros

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/book/v1` | Lista todos os livros |
| GET | `/api/book/v1/{id}` | Busca por ID |
| POST | `/api/book/v1` | Cria um livro |
| PUT | `/api/book/v1` | Atualiza um livro |
| DELETE | `/api/book/v1/{id}` | Remove um livro |

### Arquivos

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/file/v1/uploadFile` | Upload de um arquivo (PDF, JPG, JPEG, PNG) |
| POST | `/api/file/v1/uploadMultipleFile` | Upload múltiplo |
| GET | `/api/file/v1/downloadFile/{fileName}` | Download de arquivo |

> 📖 Documentação completa disponível em `/swagger` após subir a aplicação.

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.
