using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Models.Dtos;

// Conversion entre entidades de base de datos y objetos de transferencia
public static class DtoMapper
{
    public static PersonaDto ToDto(this Persona p) => new()
    {
        Cc = p.Cc,
        Nombre = p.Nombre,
        Apellido = p.Apellido,
        Genero = p.Genero,
        Edad = p.Edad
    };

    public static Persona ToEntity(this PersonaDto dto) => new()
    {
        Cc = dto.Cc,
        Nombre = dto.Nombre,
        Apellido = dto.Apellido,
        Genero = dto.Genero,
        Edad = dto.Edad
    };

    public static void CopyTo(this PersonaDto dto, Persona p)
    {
        p.Nombre = dto.Nombre;
        p.Apellido = dto.Apellido;
        p.Genero = dto.Genero;
        p.Edad = dto.Edad;
    }

    public static ProfesionDto ToDto(this Profesion p) => new()
    {
        Id = p.Id,
        Nom = p.Nom,
        Des = p.Des
    };

    public static Profesion ToEntity(this ProfesionDto dto) => new()
    {
        Id = dto.Id,
        Nom = dto.Nom,
        Des = dto.Des
    };

    public static void CopyTo(this ProfesionDto dto, Profesion p)
    {
        p.Nom = dto.Nom;
        p.Des = dto.Des;
    }

    public static EstudioDto ToDto(this Estudio e) => new()
    {
        IdProf = e.IdProf,
        CcPer = e.CcPer,
        Fecha = e.Fecha,
        Univer = e.Univer
    };

    public static Estudio ToEntity(this EstudioDto dto) => new()
    {
        IdProf = dto.IdProf,
        CcPer = dto.CcPer,
        Fecha = dto.Fecha,
        Univer = dto.Univer
    };

    public static void CopyTo(this EstudioDto dto, Estudio e)
    {
        e.Fecha = dto.Fecha;
        e.Univer = dto.Univer;
    }

    public static TelefonoDto ToDto(this Telefono t) => new()
    {
        Num = t.Num,
        Oper = t.Oper,
        Duenio = t.Duenio
    };

    public static Telefono ToEntity(this TelefonoDto dto) => new()
    {
        Num = dto.Num,
        Oper = dto.Oper,
        Duenio = dto.Duenio
    };

    public static void CopyTo(this TelefonoDto dto, Telefono t)
    {
        t.Oper = dto.Oper;
        t.Duenio = dto.Duenio;
    }
}
