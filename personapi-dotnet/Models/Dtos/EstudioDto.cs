using System.ComponentModel.DataAnnotations;

namespace personapi_dotnet.Models.Dtos;

public class EstudioDto
{
    [Required(ErrorMessage = "La profesion es obligatoria")]
    [Display(Name = "Profesion")]
    public int IdProf { get; set; }

    [Required(ErrorMessage = "La persona es obligatoria")]
    [Display(Name = "Persona")]
    public int CcPer { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Fecha de grado")]
    public DateOnly? Fecha { get; set; }

    [StringLength(50)]
    [Display(Name = "Universidad")]
    public string? Univer { get; set; }
}
