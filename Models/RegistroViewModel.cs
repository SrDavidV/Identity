using System.ComponentModel.DataAnnotations;

namespace CursoIdentity.Models
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "El correo electronico es obligatorio")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage ="La contraseña es obligatoria")]
        [StringLength(50, ErrorMessage = "El {0} debe tener minimo {2} caracteres de longitud", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
        [Required(ErrorMessage = "La confirmación de contraseña es obligatoria")]
        [Compare("Password", ErrorMessage ="Las contraseñas no coinciden")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "El Nombre es obligatorio")]
        public string Nombre { get; set; }
        public string Url { get; set; }
        public Int32 CodigoPais { get; set; }
        public string Telefono { get; set; }
        [Required(ErrorMessage = "El pais es obligatorio")]
        public string Pais { get; set; }
        [Required(ErrorMessage = "La ciudad es obligatoria")]
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        [Required(ErrorMessage ="La fecha de nacimiento es obligatoria")]
        public DateTime FechaNacimiento { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public bool Estado { get; set; }

    }
}
