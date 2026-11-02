using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Repositories;

public class ProfesionRepository : IProfesionRepository
{
    private readonly PersonaDbContext _context;

    public ProfesionRepository(PersonaDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Profesion>> GetAllAsync()
    {
        return await _context.Profesions
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .ToListAsync();
    }

    public async Task<Profesion?> GetByIdAsync(int id)
    {
        return await _context.Profesions
            .Include(p => p.Estudios)
                .ThenInclude(e => e.CcPerNavigation)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Profesions.AnyAsync(p => p.Id == id);
    }

    public async Task AddAsync(Profesion profesion)
    {
        _context.Profesions.Add(profesion);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Profesion profesion)
    {
        _context.Profesions.Update(profesion);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var profesion = await _context.Profesions.FindAsync(id);
        if (profesion == null)
        {
            return false;
        }

        _context.Estudios.RemoveRange(_context.Estudios.Where(e => e.IdProf == id));
        _context.Profesions.Remove(profesion);
        await _context.SaveChangesAsync();
        return true;
    }
}
