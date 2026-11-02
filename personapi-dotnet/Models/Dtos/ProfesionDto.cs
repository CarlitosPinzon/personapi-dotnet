using System.ComponentModel.DataAnnotations;

namespace personapi_dotnet.Models.Dtos;

public class ProfesionDto
{
    [Required(ErrorMessage = "El id es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "El id debe ser un numero positivo")]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(90)]
    [Display(Name = "Nombre")]
    public string Nom { get; set; } = string.Empty;

    [Display(Name = "Descripcion")]
    public string? Des { get; set; }
}
