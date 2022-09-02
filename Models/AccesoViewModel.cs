﻿using System.ComponentModel.DataAnnotations;

namespace CursoIdentity.Models
{
    public class AccesoViewModel
    {
        [Required(ErrorMessage = "El correo electronico es obligatorio")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
        [Display(Name = "Recordar Datos?")]
        public bool RememberMe { get; set; }
    } 
}
