using System.ComponentModel.DataAnnotations;

namespace personapi_dotnet.Models.Dtos;

public class TelefonoDto
{
    [Required(ErrorMessage = "El numero es obligatorio")]
    [StringLength(15)]
    [Display(Name = "Numero")]
    public string Num { get; set; } = string.Empty;

    [Required(ErrorMessage = "El operador es obligatorio")]
    [StringLength(45)]
    [Display(Name = "Operador")]
    public string Oper { get; set; } = string.Empty;

    [Required(ErrorMessage = "El dueno es obligatorio")]
    [Display(Name = "Dueno")]
    public int Duenio { get; set; }
}
