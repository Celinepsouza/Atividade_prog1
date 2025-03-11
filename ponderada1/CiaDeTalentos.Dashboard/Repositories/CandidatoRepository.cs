using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CandidatoDataRepository : ICandidatoRepository
{
    private readonly string _connectionString;

    public CandidatoDataRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
                            ?? throw new ArgumentNullException(nameof(_connectionString), "A string de conexão não pode ser nula.");
    }

    // Método para obter todos os candidatos com idade e região
    public async Task<IEnumerable<Candidato>> ObterTodosAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Candidato>(
            "SELECT \"idade\", \"regiao\" FROM ciatalento LIMIT 10"
        );
    }

    // Método para obter candidatos por faixa etária
    public async Task<Dictionary<string, int>> ObterCandidatosPorFaixaEtariaAsync()
{
    using var connection = new NpgsqlConnection(_connectionString);
    var result = await connection.QueryAsync<(string, int)>(
        @"SELECT 
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




    // Método para obter candidatos por região
    public async Task<Dictionary<string, int>> ObterCandidatosPorRegiaoAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var result = await connection.QueryAsync<(string, int)>( 
            @"SELECT regiao, COUNT(*) 
              FROM (SELECT regiao FROM baseinteli LIMIT 10) AS subquery
              GROUP BY regiao
              ORDER BY COUNT(*) DESC"
        );

        return result.ToDictionary(row => row.Item1, row => row.Item2);
    }

    // Método para obter candidatos por curso
    public async Task<Dictionary<string, int>> ObterCandidatosPorCursoAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var result = await connection.QueryAsync<(string, int)>(
            @"SELECT curso, COUNT(*) 
              FROM (SELECT curso FROM baseinteli LIMIT 10) AS subquery
              GROUP BY curso
              ORDER BY COUNT(*) DESC"
        );

        return result.ToDictionary(row => row.Item1, row => row.Item2);
    }
}
