using System.ComponentModel.DataAnnotations;

namespace CursoIdentity.Models
{
    public class OlvidoPasswordViewModel
    {
        [Required(ErrorMessage ="El Correo es obligatorio")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
