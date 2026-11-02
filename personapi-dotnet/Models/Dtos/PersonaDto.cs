using System.ComponentModel.DataAnnotations;

namespace personapi_dotnet.Models.Dtos;

public class PersonaDto
{
    [Required(ErrorMessage = "La cedula es obligatoria")]
    [Range(1, int.MaxValue, ErrorMessage = "La cedula debe ser un numero positivo")]
    [Display(Name = "Cedula")]
    public int Cc { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(45)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(45)]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El genero es obligatorio")]
    [RegularExpression("^[MF]$", ErrorMessage = "El genero debe ser M o F")]
    [Display(Name = "Genero")]
    public string Genero { get; set; } = string.Empty;

    [Range(0, 150, ErrorMessage = "La edad debe estar entre 0 y 150")]
    public int? Edad { get; set; }
}
