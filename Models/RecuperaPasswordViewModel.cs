using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CursoIdentity.Models
{
    public class RecuperaPasswordViewModel
    {
        [Required(ErrorMessage = "El correo electronico es obligatorio")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
        [Required(ErrorMessage = "La confirmación de contraseña es obligatoria")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }    
 
    }
}
