using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Dtos;

namespace personapi_dotnet.Controllers;

public class ProfesionesController : Controller
{
    private readonly IProfesionRepository _profesionRepository;

    public ProfesionesController(IProfesionRepository profesionRepository)
    {
        _profesionRepository = profesionRepository;
    }

    // GET: Profesiones
    public async Task<IActionResult> Index()
    {
        return View(await _profesionRepository.GetAllAsync());
    }

    // GET: Profesiones/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var profesion = await _profesionRepository.GetByIdAsync(id);
        if (profesion == null)
        {
            return NotFound();
        }
        return View(profesion);
    }

    // GET: Profesiones/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Profesiones/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProfesionDto dto)
    {
        if (ModelState.IsValid && await _profesionRepository.ExistsAsync(dto.Id))
        {
            ModelState.AddModelError(nameof(dto.Id), "Ya existe una profesion con ese id");
        }

        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        await _profesionRepository.AddAsync(dto.ToEntity());
        TempData["Mensaje"] = "Profesion creada correctamente";
        return RedirectToAction(nameof(Index));
    }

    // GET: Profesiones/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var profesion = await _profesionRepository.GetByIdAsync(id);
        if (profesion == null)
        {
            return NotFound();
        }
        return View(profesion.ToDto());
    }

    // POST: Profesiones/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProfesionDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var profesion = await _profesionRepository.GetByIdAsync(id);
        if (profesion == null)
        {
            return NotFound();
        }

        dto.CopyTo(profesion);
        await _profesionRepository.UpdateAsync(profesion);
        TempData["Mensaje"] = "Profesion actualizada correctamente";
        return RedirectToAction(nameof(Index));
    }

    // GET: Profesiones/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var profesion = await _profesionRepository.GetByIdAsync(id);
        if (profesion == null)
        {
            return NotFound();
        }
        return View(profesion);
    }

    // POST: Profesiones/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _profesionRepository.DeleteAsync(id);
        TempData["Mensaje"] = "Profesion eliminada correctamente";
        return RedirectToAction(nameof(Index));
    }
}
