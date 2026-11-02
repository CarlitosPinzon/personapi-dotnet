using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Dtos;

namespace personapi_dotnet.Controllers;

public class PersonasController : Controller
{
    private readonly IPersonaRepository _personaRepository;

    public PersonasController(IPersonaRepository personaRepository)
    {
        _personaRepository = personaRepository;
    }

    // GET: Personas
    public async Task<IActionResult> Index()
    {
        return View(await _personaRepository.GetAllAsync());
    }

    // GET: Personas/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var persona = await _personaRepository.GetByIdAsync(id);
        if (persona == null)
        {
            return NotFound();
        }
        return View(persona);
    }

    // GET: Personas/Create
    public IActionResult Create()
    {
        CargarGeneros();
        return View();
    }

    // POST: Personas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PersonaDto dto)
    {
        if (ModelState.IsValid && await _personaRepository.ExistsAsync(dto.Cc))
        {
            ModelState.AddModelError(nameof(dto.Cc), "Ya existe una persona con esa cedula");
        }

        if (!ModelState.IsValid)
        {
            CargarGeneros();
            return View(dto);
        }

        await _personaRepository.AddAsync(dto.ToEntity());
        TempData["Mensaje"] = "Persona creada correctamente";
        return RedirectToAction(nameof(Index));
    }

    // GET: Personas/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var persona = await _personaRepository.GetByIdAsync(id);
        if (persona == null)
        {
            return NotFound();
        }
        CargarGeneros();
        return View(persona.ToDto());
    }

    // POST: Personas/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PersonaDto dto)
    {
        if (id != dto.Cc)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            CargarGeneros();
            return View(dto);
        }

        var persona = await _personaRepository.GetByIdAsync(id);
        if (persona == null)
        {
            return NotFound();
        }

        dto.CopyTo(persona);
        await _personaRepository.UpdateAsync(persona);
        TempData["Mensaje"] = "Persona actualizada correctamente";
        return RedirectToAction(nameof(Index));
    }

    // GET: Personas/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var persona = await _personaRepository.GetByIdAsync(id);
        if (persona == null)
        {
            return NotFound();
        }
        return View(persona);
    }

    // POST: Personas/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _personaRepository.DeleteAsync(id);
        TempData["Mensaje"] = "Persona eliminada correctamente";
        return RedirectToAction(nameof(Index));
    }

    private void CargarGeneros()
    {
        ViewBag.Generos = new SelectList(new[]
        {
            new { Valor = "M", Texto = "Masculino" },
            new { Valor = "F", Texto = "Femenino" }
        }, "Valor", "Texto");
    }
}
