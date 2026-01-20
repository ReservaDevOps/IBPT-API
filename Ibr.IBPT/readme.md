# Ibr.IBPT

API para upload/consulta da tabela IBPT e calculo de tributos associados, com suporte a PostgreSQL (prod) e SQLite (dev).

## Destaques
- Swagger pronto para exploracao e teste de endpoints.
- EF Core com chave primaria composta para a tabela IBPT.
- Modo dev sem Postgres local (SQLite auto-criado).

## Stack
- .NET 10
- ASP.NET Core + EF Core
- PostgreSQL (producao) ou SQLite (desenvolvimento)

## Requisitos
- .NET SDK 10.0+
- PostgreSQL somente se for rodar em modo Postgres

## Configuracao
Chaves principais em `Ibr.IBPT/appsettings.json` e `Ibr.IBPT/appsettings.Development.json`:

- `DatabaseProvider`: `Postgres` ou `Sqlite`
- `ConnectionStrings:Database`: string do Postgres
- `ConnectionStrings:Sqlite`: arquivo SQLite local

Tambem pode usar variaveis de ambiente:
- `DatabaseProvider`
- `ConnectionStrings__Database`
- `ConnectionStrings__Sqlite`

## Como rodar (dev com SQLite)
1) `dotnet restore`
2) `dotnet run --project Ibr.IBPT/Ibr.IBPT.csproj`
3) Swagger em `/swagger`

No modo SQLite, o arquivo `ibpt.dev.db` e criado automaticamente na pasta de execucao do projeto.

## Como rodar (prod/ambiente com Postgres)
1) Ajuste `DatabaseProvider` para `Postgres`
2) Configure `ConnectionStrings:Database`
3) Execute o app:
   - `dotnet run --project Ibr.IBPT/Ibr.IBPT.csproj`

Para aplicar migrations no Postgres (se necessario):
```bash
dotnet tool install -g dotnet-ef
dotnet ef database update --project Ibr.IBPT/Ibr.IBPT.csproj
```

## Observacoes
- Em SQLite usamos `EnsureCreated` na inicializacao, sem aplicar migrations.
- A API nao possui consumers; todo processamento e via HTTP.
