using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Dtos;

namespace personapi_dotnet.Controllers.Api;

[ApiController]
[Tags("Personas")]
[Route("api/personas")]
[Produces("application/json")]
public class PersonaApiController : ControllerBase
{
    private readonly IPersonaRepository _personaRepository;

    public PersonaApiController(IPersonaRepository personaRepository)
    {
        _personaRepository = personaRepository;
    }

    /// <summary>Lista todas las personas.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PersonaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PersonaDto>>> GetAll()
    {
        var personas = await _personaRepository.GetAllAsync();
        return Ok(personas.Select(p => p.ToDto()));
    }

    /// <summary>Consulta una persona por su cedula.</summary>
    [HttpGet("{cc:int}")]
    [ProducesResponseType(typeof(PersonaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonaDto>> GetById(int cc)
    {
        var persona = await _personaRepository.GetByIdAsync(cc);
        if (persona == null)
        {
            return NotFound();
        }
        return Ok(persona.ToDto());
    }

    /// <summary>Crea una persona.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(PersonaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PersonaDto>> Create(PersonaDto dto)
    {
        if (await _personaRepository.ExistsAsync(dto.Cc))
        {
            return Conflict($"Ya existe una persona con cedula {dto.Cc}");
        }

        await _personaRepository.AddAsync(dto.ToEntity());
        return CreatedAtAction(nameof(GetById), new { cc = dto.Cc }, dto);
    }

    /// <summary>Actualiza los datos de una persona.</summary>
    [HttpPut("{cc:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int cc, PersonaDto dto)
    {
        if (cc != dto.Cc)
        {
            return BadRequest("La cedula de la URL no coincide con la del cuerpo");
        }

        var persona = await _personaRepository.GetByIdAsync(cc);
        if (persona == null)
        {
            return NotFound();
        }

        dto.CopyTo(persona);
        await _personaRepository.UpdateAsync(persona);
        return NoContent();
    }

    /// <summary>Elimina una persona junto con sus telefonos y estudios.</summary>
    [HttpDelete("{cc:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int cc)
    {
        var eliminado = await _personaRepository.DeleteAsync(cc);
        return eliminado ? NoContent() : NotFound();
    }
}
