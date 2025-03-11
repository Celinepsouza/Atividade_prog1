using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICandidatoService
{
    Task<Dictionary<string, int>> ObterCandidatosPorFaixaEtariaAsync();
    Task<Dictionary<string, int>> ObterCandidatosPorRegiaoAsync();
    Task<Dictionary<string, int>> ObterCandidatosPorCursoAsync();
}