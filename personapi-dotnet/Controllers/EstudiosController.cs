using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Dtos;

namespace personapi_dotnet.Controllers;

public class EstudiosController : Controller
{
    private readonly IEstudioRepository _estudioRepository;
    private readonly IPersonaRepository _personaRepository;
    private readonly IProfesionRepository _profesionRepository;

    public EstudiosController(
        IEstudioRepository estudioRepository,
        IPersonaRepository personaRepository,
        IProfesionRepository profesionRepository)
    {
        _estudioRepository = estudioRepository;
        _personaRepository = personaRepository;
        _profesionRepository = profesionRepository;
    }

    // GET: Estudios
    public async Task<IActionResult> Index()
    {
        return View(await _estudioRepository.GetAllAsync());
    }

    // GET: Estudios/Details?idProf=1&ccPer=123
    public async Task<IActionResult> Details(int idProf, int ccPer)
    {
        var estudio = await _estudioRepository.GetByIdAsync(idProf, ccPer);
        if (estudio == null)
        {
            return NotFound();
        }
        return View(estudio);
    }

    // GET: Estudios/Create
    public async Task<IActionResult> Create()
    {
        await CargarListas();
        return View(new EstudioDto());
    }

    // POST: Estudios/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EstudioDto dto)
    {
        if (ModelState.IsValid && await _estudioRepository.ExistsAsync(dto.IdProf, dto.CcPer))
        {
            ModelState.AddModelError(string.Empty, "La persona ya tiene registrado ese estudio");
        }

        if (!ModelState.IsValid)
        {
            await CargarListas(dto.IdProf, dto.CcPer);
            return View(dto);
        }

        await _estudioRepository.AddAsync(dto.ToEntity());
        TempData["Mensaje"] = "Estudio registrado correctamente";
        return RedirectToAction(nameof(Index));
    }

    // GET: Estudios/Edit?idProf=1&ccPer=123
    public async Task<IActionResult> Edit(int idProf, int ccPer)
    {
        var estudio = await _estudioRepository.GetByIdAsync(idProf, ccPer);
        if (estudio == null)
        {
            return NotFound();
        }
        ViewBag.NombrePersona = $"{estudio.CcPerNavigation.Nombre} {estudio.CcPerNavigation.Apellido}";
        ViewBag.NombreProfesion = estudio.IdProfNavigation.Nom;
        return View(estudio.ToDto());
    }

    // POST: Estudios/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EstudioDto dto)
    {
        var estudio = await _estudioRepository.GetByIdAsync(dto.IdProf, dto.CcPer);
        if (estudio == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewBag.NombrePersona = $"{estudio.CcPerNavigation.Nombre} {estudio.CcPerNavigation.Apellido}";
            ViewBag.NombreProfesion = estudio.IdProfNavigation.Nom;
            return View(dto);
        }

        dto.CopyTo(estudio);
        await _estudioRepository.UpdateAsync(estudio);
        TempData["Mensaje"] = "Estudio actualizado correctamente";
        return RedirectToAction(nameof(Index));
    }

    // GET: Estudios/Delete?idProf=1&ccPer=123
    public async Task<IActionResult> Delete(int idProf, int ccPer)
    {
        var estudio = await _estudioRepository.GetByIdAsync(idProf, ccPer);
        if (estudio == null)
        {
            return NotFound();
        }
        return View(estudio);
    }

    // POST: Estudios/Delete
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int idProf, int ccPer)
    {
        await _estudioRepository.DeleteAsync(idProf, ccPer);
        TempData["Mensaje"] = "Estudio eliminado correctamente";
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarListas(int? idProf = null, int? ccPer = null)
    {
        var personas = (await _personaRepository.GetAllAsync())
            .Select(p => new { p.Cc, Nombre = $"{p.Cc} - {p.Nombre} {p.Apellido}" });
        var profesiones = await _profesionRepository.GetAllAsync();

        ViewBag.Personas = new SelectList(personas, "Cc", "Nombre", ccPer);
        ViewBag.Profesiones = new SelectList(profesiones, "Id", "Nom", idProf);
    }
}
