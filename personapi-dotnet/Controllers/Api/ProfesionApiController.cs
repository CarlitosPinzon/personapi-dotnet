using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Dtos;

namespace personapi_dotnet.Controllers.Api;

[ApiController]
[Tags("Profesiones")]
[Route("api/profesiones")]
[Produces("application/json")]
public class ProfesionApiController : ControllerBase
{
    private readonly IProfesionRepository _profesionRepository;

    public ProfesionApiController(IProfesionRepository profesionRepository)
    {
        _profesionRepository = profesionRepository;
    }

    /// <summary>Lista todas las profesiones.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProfesionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProfesionDto>>> GetAll()
    {
        var profesiones = await _profesionRepository.GetAllAsync();
        return Ok(profesiones.Select(p => p.ToDto()));
    }

    /// <summary>Consulta una profesion por su id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProfesionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfesionDto>> GetById(int id)
    {
        var profesion = await _profesionRepository.GetByIdAsync(id);
        if (profesion == null)
        {
            return NotFound();
        }
        return Ok(profesion.ToDto());
    }

    /// <summary>Crea una profesion.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProfesionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProfesionDto>> Create(ProfesionDto dto)
    {
        if (await _profesionRepository.ExistsAsync(dto.Id))
        {
            return Conflict($"Ya existe una profesion con id {dto.Id}");
        }

        await _profesionRepository.AddAsync(dto.ToEntity());
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    /// <summary>Actualiza una profesion.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, ProfesionDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest("El id de la URL no coincide con el del cuerpo");
        }

        var profesion = await _profesionRepository.GetByIdAsync(id);
        if (profesion == null)
        {
            return NotFound();
        }

        dto.CopyTo(profesion);
        await _profesionRepository.UpdateAsync(profesion);
        return NoContent();
    }

    /// <summary>Elimina una profesion y los estudios asociados.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _profesionRepository.DeleteAsync(id);
        return eliminado ? NoContent() : NotFound();
    }
}
