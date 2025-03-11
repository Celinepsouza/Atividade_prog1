using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/candidatos")] // 🔹 Rota para acessar os dados de candidatos
public class CandidatoController : ControllerBase
{
    private readonly ICandidatoService _candidatoService;

    public CandidatoController(ICandidatoService candidatoService)
    {
        _candidatoService = candidatoService;
    }

    // Endpoint para obter candidatos por faixa etária
    [HttpGet("faixa-etaria")]
    public async Task<IActionResult> ObterCandidatosPorFaixaEtaria()
    {
        var resultado = await _candidatoService.ObterCandidatosPorFaixaEtariaAsync();
        return Ok(resultado);
    }

    // Endpoint para obter candidatos por região
    [HttpGet("regiao")]
    public async Task<IActionResult> ObterCandidatosPorRegiao()
    {
        var resultado = await _candidatoService.ObterCandidatosPorRegiaoAsync();
        return Ok(resultado);
    }

    // Endpoint para obter candidatos por curso
    [HttpGet("curso")]
    public async Task<IActionResult> ObterCandidatosPorCurso()
    {
        var resultado = await _candidatoService.ObterCandidatosPorCursoAsync();
        return Ok(resultado);
    }
}
