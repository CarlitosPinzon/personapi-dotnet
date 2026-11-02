using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Dtos;

namespace personapi_dotnet.Controllers.Api;

[ApiController]
[Tags("Telefonos")]
[Route("api/telefonos")]
[Produces("application/json")]
public class TelefonoApiController : ControllerBase
{
    private readonly ITelefonoRepository _telefonoRepository;
    private readonly IPersonaRepository _personaRepository;

    public TelefonoApiController(ITelefonoRepository telefonoRepository, IPersonaRepository personaRepository)
    {
        _telefonoRepository = telefonoRepository;
        _personaRepository = personaRepository;
    }

    /// <summary>Lista todos los telefonos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TelefonoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TelefonoDto>>> GetAll()
    {
        var telefonos = await _telefonoRepository.GetAllAsync();
        return Ok(telefonos.Select(t => t.ToDto()));
    }

    /// <summary>Consulta un telefono por su numero.</summary>
    [HttpGet("{num}")]
    [ProducesResponseType(typeof(TelefonoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TelefonoDto>> GetById(string num)
    {
        var telefono = await _telefonoRepository.GetByIdAsync(num);
        if (telefono == null)
        {
            return NotFound();
        }
        return Ok(telefono.ToDto());
    }

    /// <summary>Registra un telefono.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TelefonoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TelefonoDto>> Create(TelefonoDto dto)
    {
        if (!await _personaRepository.ExistsAsync(dto.Duenio))
        {
            return BadRequest($"No existe la persona con cedula {dto.Duenio}");
        }
        if (await _telefonoRepository.ExistsAsync(dto.Num))
        {
            return Conflict($"El telefono {dto.Num} ya esta registrado");
        }

        await _telefonoRepository.AddAsync(dto.ToEntity());
        return CreatedAtAction(nameof(GetById), new { num = dto.Num }, dto);
    }

    /// <summary>Actualiza el operador o el dueno de un telefono.</summary>
    [HttpPut("{num}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string num, TelefonoDto dto)
    {
        if (num != dto.Num)
        {
            return BadRequest("El numero de la URL no coincide con el del cuerpo");
        }

        var telefono = await _telefonoRepository.GetByIdAsync(num);
        if (telefono == null)
        {
            return NotFound();
        }
        if (!await _personaRepository.ExistsAsync(dto.Duenio))
        {
            return BadRequest($"No existe la persona con cedula {dto.Duenio}");
        }

        dto.CopyTo(telefono);
        await _telefonoRepository.UpdateAsync(telefono);
        return NoContent();
    }

    /// <summary>Elimina un telefono.</summary>
    [HttpDelete("{num}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string num)
    {
        var eliminado = await _telefonoRepository.DeleteAsync(num);
        return eliminado ? NoContent() : NotFound();
    }
}
