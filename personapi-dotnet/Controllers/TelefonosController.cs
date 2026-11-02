using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Dtos;

namespace personapi_dotnet.Controllers;

public class TelefonosController : Controller
{
    private readonly ITelefonoRepository _telefonoRepository;
    private readonly IPersonaRepository _personaRepository;

    public TelefonosController(ITelefonoRepository telefonoRepository, IPersonaRepository personaRepository)
    {
        _telefonoRepository = telefonoRepository;
        _personaRepository = personaRepository;
    }

    // GET: Telefonos
    public async Task<IActionResult> Index()
    {
        return View(await _telefonoRepository.GetAllAsync());
    }

    // GET: Telefonos/Details/3001234567
    public async Task<IActionResult> Details(string id)
    {
        var telefono = await _telefonoRepository.GetByIdAsync(id);
        if (telefono == null)
        {
            return NotFound();
        }
        return View(telefono);
    }

    // GET: Telefonos/Create
    public async Task<IActionResult> Create()
    {
        await CargarPersonas();
        return View(new TelefonoDto());
    }

    // POST: Telefonos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TelefonoDto dto)
    {
        if (ModelState.IsValid && await _telefonoRepository.ExistsAsync(dto.Num))
        {
            ModelState.AddModelError(nameof(dto.Num), "Ese numero ya esta registrado");
        }

        if (!ModelState.IsValid)
        {
            await CargarPersonas(dto.Duenio);
            return View(dto);
        }

        await _telefonoRepository.AddAsync(dto.ToEntity());
        TempData["Mensaje"] = "Telefono registrado correctamente";
        return RedirectToAction(nameof(Index));
    }

    // GET: Telefonos/Edit/3001234567
    public async Task<IActionResult> Edit(string id)
    {
        var telefono = await _telefonoRepository.GetByIdAsync(id);
        if (telefono == null)
        {
            return NotFound();
        }
        await CargarPersonas(telefono.Duenio);
        return View(telefono.ToDto());
    }

    // POST: Telefonos/Edit/3001234567
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, TelefonoDto dto)
    {
        if (id != dto.Num)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            await CargarPersonas(dto.Duenio);
            return View(dto);
        }

        var telefono = await _telefonoRepository.GetByIdAsync(id);
        if (telefono == null)
        {
            return NotFound();
        }

        dto.CopyTo(telefono);
        await _telefonoRepository.UpdateAsync(telefono);
        TempData["Mensaje"] = "Telefono actualizado correctamente";
        return RedirectToAction(nameof(Index));
    }

    // GET: Telefonos/Delete/3001234567
    public async Task<IActionResult> Delete(string id)
    {
        var telefono = await _telefonoRepository.GetByIdAsync(id);
        if (telefono == null)
        {
            return NotFound();
        }
        return View(telefono);
    }

    // POST: Telefonos/Delete/3001234567
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        await _telefonoRepository.DeleteAsync(id);
        TempData["Mensaje"] = "Telefono eliminado correctamente";
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarPersonas(int? seleccionada = null)
    {
        var personas = (await _personaRepository.GetAllAsync())
            .Select(p => new { p.Cc, Nombre = $"{p.Cc} - {p.Nombre} {p.Apellido}" });
        ViewBag.Personas = new SelectList(personas, "Cc", "Nombre", seleccionada);
    }
}
