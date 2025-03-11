# Atividade_prog1

### Model: Representa os dados
O Model define como os dados são estruturados na aplicação. No seu caso, temos a classe Candidato que define as propriedades que representam as informações de um candidato.

Exemplo do Model Candidato:
```csharp
public class Candidato
{
    public int CandidatoId { get; set; }
    public int Idade { get; set; }
    public string RegiaoResidencial { get; set; } = string.Empty;
    public string Curso { get; set; } = string.Empty;
}
```
CandidatoId: ID único do candidato.

Idade: Idade do candidato.

RegiaoResidencial: Região onde o candidato reside.

Curso: Curso em que o candidato está matriculado ou relacionado.

Este Model será utilizado em várias partes do código, principalmente nas operações do Repository e Service, para manipular e transferir dados.

### Repository: A camada responsável pelo acesso aos dados
O Repository é uma abstração sobre o acesso ao banco de dados. Ele encapsula as consultas ao banco e os mapeia para objetos do tipo Candidato ou outro tipo relevante. No seu código, o Repository utiliza o Dapper para mapear os resultados da consulta SQL para o Model Candidato.

Exemplo de CandidatoDataRepository:
```csharp
public class CandidatoDataRepository : ICandidatoRepository
{
    private readonly string _connectionString;

    public CandidatoDataRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
                            ?? throw new ArgumentNullException(nameof(_connectionString), "A string de conexão não pode ser nula.");
    }

    // Método para obter candidatos por faixa etária
    public async Task<Dictionary<string, int>> ObterCandidatosPorFaixaEtariaAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var result = await connection.QueryAsync<(string, int)>(@" 
            SELECT 
                CASE 
                    WHEN CAST(idade AS integer) BETWEEN 18 AND 24 THEN '18-24'
                    WHEN CAST(idade AS integer) BETWEEN 25 AND 34 THEN '25-34'
                    WHEN CAST(idade AS integer) BETWEEN 35 AND 44 THEN '35-44'
                    WHEN CAST(idade AS integer) BETWEEN 45 AND 54 THEN '45-54'
                    ELSE '55+' 
                END AS FaixaEtaria,
                COUNT(*) 
            FROM (SELECT idade FROM baseinteli LIMIT 10) AS subquery
            GROUP BY FaixaEtaria
            ORDER BY COUNT(*) DESC"
        );

        return result.ToDictionary(row => row.Item1, row => row.Item2);
    }
}
```
O Repository possui métodos que fazem consultas ao banco e retornam os dados. Aqui, o método ObterCandidatosPorFaixaEtariaAsync executa uma consulta SQL que retorna a quantidade de candidatos por faixa etária, e o Dapper é usado para mapear o resultado da consulta para um formato Dictionary<string, int>, onde a chave é a faixa etária e o valor é o número de candidatos.

### Service: A camada que contém a lógica de negócios
O Service é responsável por orquestrar a lógica de negócios. Ele chama o Repository para obter dados e, em seguida, processa ou transforma esses dados, se necessário, antes de retorná-los para o Controller.

Exemplo de CandidatoService:
```csharp
public class CandidatoService : ICandidatoService
{
    private readonly ICandidatoRepository _candidatoRepositorio;

    public CandidatoService(ICandidatoRepository candidatoRepositorio)
    {
        _candidatoRepositorio = candidatoRepositorio;
    }

    public async Task<Dictionary<string, int>> ObterCandidatosPorFaixaEtariaAsync()
    {
        return await _candidatoRepositorio.ObterCandidatosPorFaixaEtariaAsync();
    }

    public async Task<Dictionary<string, int>> ObterCandidatosPorRegiaoAsync()
    {
        return await _candidatoRepositorio.ObterCandidatosPorRegiaoAsync();
    }

    public async Task<Dictionary<string, int>> ObterCandidatosPorCursoAsync()
    {
        return await _candidatoRepositorio.ObterCandidatosPorCursoAsync();
    }
}
```

O Service expõe métodos que podem ser usados pelo Controller. Ele chama os métodos do Repository e devolve os dados para o Controller. A principal responsabilidade do Service é orquestrar as chamadas ao Repository e tratar a lógica de negócios, caso necessário.

### Como eles se integram:
- Controller: O Controller expõe a API (via HTTP) para que o cliente (frontend ou outro sistema) possa interagir com a aplicação. Ele recebe a requisição, chama o Service e retorna a resposta.

- Service: O Service contém a lógica de negócios. Ele é quem vai processar as informações de maneira mais complexa e chamar o Repository para acessar os dados diretamente. Após obter os dados do Repository, o Service pode aplicar qualquer lógica adicional necessária (como filtros, transformações, etc.).

- Repository: O Repository é a camada que interage diretamente com o banco de dados. Ele executa as consultas SQL e mapeia os resultados para objetos de modelo (por exemplo, Candidato). O Repository encapsula os detalhes de como os dados são armazenados, garantindo que a camada de serviço ou controller não precise se preocupar com isso.