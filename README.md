# Desafio Dev Jr - Seed IBPT 2025

Temos um diretorio com CSVs reais da tabela IBPT 2025 em:
`Ibr.IBPT/Data/Seed/2025`

Seu desafio e criar um seed para carregar esses arquivos no banco em ambiente de development.

## Resultado esperado
- Ao iniciar a API em `Development`, os dados dos CSVs 2025 sao carregados no banco.
- O seed deve ser idempotente (rodar mais de uma vez nao duplica dados).
- O seed deve respeitar a regra de UF no nome do arquivo e o layout CSV usado pela API.
- Ao final, a API deve responder consultas com dados reais:
  - Exemplo: `GET /api/v1/ibpt?uf=SP&code=01012100` deve retornar um item valido.

## Regras e dicas
- Reaproveite a logica de leitura/limpeza de CSV ja usada no endpoint `POST /api/v1/ibpt`.
- Os CSVs usam `;` como separador e headers com nomes padronizados.
- Nao use dependencias externas alem das ja presentes no projeto.
- Se preferir, deixe o seed condicionado a uma flag de configuracao (ex.: `SeedData:Enabled`).

## Entregaveis
- Codigo do seed (ex.: em `Ibr.IBPT/Data/Seed`).
- Ajustes no startup para executar o seed em `Development`.
- Pequena nota no README do serviço explicando como ativar o seed.
