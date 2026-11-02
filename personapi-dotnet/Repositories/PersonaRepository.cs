using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Repositories;

public class PersonaRepository : IPersonaRepository
{
    private readonly PersonaDbContext _context;

    public PersonaRepository(PersonaDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Persona>> GetAllAsync()
    {
        return await _context.Personas
            .AsNoTracking()
            .OrderBy(p => p.Apellido)
            .ThenBy(p => p.Nombre)
            .ToListAsync();
    }

    public async Task<Persona?> GetByIdAsync(int cc)
    {
        return await _context.Personas
            .Include(p => p.Telefonos)
            .Include(p => p.Estudios)
                .ThenInclude(e => e.IdProfNavigation)
            .FirstOrDefaultAsync(p => p.Cc == cc);
    }

    public async Task<bool> ExistsAsync(int cc)
    {
        return await _context.Personas.AnyAsync(p => p.Cc == cc);
    }

    public async Task AddAsync(Persona persona)
    {
        _context.Personas.Add(persona);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Persona persona)
    {
        _context.Personas.Update(persona);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int cc)
    {
        var persona = await _context.Personas.FindAsync(cc);
        if (persona == null)
        {
            return false;
        }

        // Se eliminan primero los registros que dependen de la persona
        _context.Telefonos.RemoveRange(_context.Telefonos.Where(t => t.Duenio == cc));
        _context.Estudios.RemoveRange(_context.Estudios.Where(e => e.CcPer == cc));
        _context.Personas.Remove(persona);
        await _context.SaveChangesAsync();
        return true;
    }
}
