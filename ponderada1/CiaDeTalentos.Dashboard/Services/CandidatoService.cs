using System.Collections.Generic;
using System.Threading.Tasks;

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
