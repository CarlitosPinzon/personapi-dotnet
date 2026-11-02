using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Dtos;

namespace personapi_dotnet.Controllers.Api;

[ApiController]
[Tags("Estudios")]
[Route("api/estudios")]
[Produces("application/json")]
public class EstudioApiController : ControllerBase
{
    private readonly IEstudioRepository _estudioRepository;
    private readonly IPersonaRepository _personaRepository;
    private readonly IProfesionRepository _profesionRepository;

    public EstudioApiController(
        IEstudioRepository estudioRepository,
        IPersonaRepository personaRepository,
        IProfesionRepository profesionRepository)
    {
        _estudioRepository = estudioRepository;
        _personaRepository = personaRepository;
        _profesionRepository = profesionRepository;
    }

    /// <summary>Lista todos los estudios.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EstudioDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EstudioDto>>> GetAll()
    {
        var estudios = await _estudioRepository.GetAllAsync();
        return Ok(estudios.Select(e => e.ToDto()));
    }

    /// <summary>Consulta un estudio por profesion y cedula de la persona.</summary>
    [HttpGet("{idProf:int}/{ccPer:int}")]
    [ProducesResponseType(typeof(EstudioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EstudioDto>> GetById(int idProf, int ccPer)
    {
        var estudio = await _estudioRepository.GetByIdAsync(idProf, ccPer);
        if (estudio == null)
        {
            return NotFound();
        }
        return Ok(estudio.ToDto());
    }

    /// <summary>Registra un estudio de una persona.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(EstudioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<EstudioDto>> Create(EstudioDto dto)
    {
        if (!await _personaRepository.ExistsAsync(dto.CcPer))
        {
            return BadRequest($"No existe la persona con cedula {dto.CcPer}");
        }
        if (!await _profesionRepository.ExistsAsync(dto.IdProf))
        {
            return BadRequest($"No existe la profesion con id {dto.IdProf}");
        }
        if (await _estudioRepository.ExistsAsync(dto.IdProf, dto.CcPer))
        {
            return Conflict("La persona ya tiene registrado ese estudio");
        }

        await _estudioRepository.AddAsync(dto.ToEntity());
        return CreatedAtAction(nameof(GetById), new { idProf = dto.IdProf, ccPer = dto.CcPer }, dto);
    }

    /// <summary>Actualiza la fecha y universidad de un estudio.</summary>
    [HttpPut("{idProf:int}/{ccPer:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int idProf, int ccPer, EstudioDto dto)
    {
        if (idProf != dto.IdProf || ccPer != dto.CcPer)
        {
            return BadRequest("La llave de la URL no coincide con la del cuerpo");
        }

        var estudio = await _estudioRepository.GetByIdAsync(idProf, ccPer);
        if (estudio == null)
        {
            return NotFound();
        }

        dto.CopyTo(estudio);
        await _estudioRepository.UpdateAsync(estudio);
        return NoContent();
    }

    /// <summary>Elimina un estudio.</summary>
    [HttpDelete("{idProf:int}/{ccPer:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int idProf, int ccPer)
    {
        var eliminado = await _estudioRepository.DeleteAsync(idProf, ccPer);
        return eliminado ? NoContent() : NotFound();
    }
}
