using System.ComponentModel.DataAnnotations;

public class MiViewModel
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string? Nombre { get; set; }

    [StringLength(50, ErrorMessage = "El nombre debe tener entre {2} y {1} caracteres", MinimumLength = 2)]
    public string? Nombreb { get; set; }

    [Range(1, 100, ErrorMessage = "La edad debe estar entre 1 y 100")]
    public int Edad { get; set; }

    [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Formato de código postal inválido")]
    public string? CodigoPostal { get; set; }

    [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido")]
    public string? CorreoElectronico { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public string? Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
    public string? ConfirmarPassword { get; set; }

    [DataType(DataType.Date)]
    public DateTime Fecha { get; set; }



    [CustomValidation(typeof(MiValidador), "ValidarNombre")]
    public string? Nombrc { get; set; }
}

public class MiValidador
{
    public static ValidationResult ValidarNombre(string nombre, ValidationContext context)
    {
        if (nombre != null && nombre.ToLower() == "admin")
        {
            return new ValidationResult("El nombre 'admin' no está permitido");
        }
        return ValidationResult.Success;
    }
}
