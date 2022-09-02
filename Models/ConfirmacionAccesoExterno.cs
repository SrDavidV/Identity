using System.ComponentModel.DataAnnotations;

namespace CursoIdentity.Models
{
    public class ConfirmacionAccesoExterno
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
