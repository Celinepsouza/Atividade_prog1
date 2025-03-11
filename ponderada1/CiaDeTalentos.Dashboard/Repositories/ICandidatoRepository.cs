using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICandidatoRepository
{
    Task<IEnumerable<Candidato>> ObterTodosAsync();
    Task<Dictionary<string, int>> ObterCandidatosPorFaixaEtariaAsync();
    Task<Dictionary<string, int>> ObterCandidatosPorRegiaoAsync();
    Task<Dictionary<string, int>> ObterCandidatosPorCursoAsync();
}
