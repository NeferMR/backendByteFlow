using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AseguradosAPI.Models
{

    // Creación de la clase Asegurado para el modelo de datos Asegurados con los atributos requeridos según el problema
    public class Asegurado
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // se especifica que no se generará una clave primaria automáticamente
        public int NumeroIdentificacion { get; set; }

        [Required(ErrorMessage = "El primer nombre es obligatorio.")]
        public string PrimerNombre { get; set; }

        public string? SegundoNombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        public string PrimerApellido { get; set; }

        [Required(ErrorMessage = "El segundo apellido es obligatorio.")]
        public string SegundoApellido { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El valor del seguro es obligatorio.")]
        public decimal ValorSeguro { get; set; }

        public string? Observaciones { get; set; }
    }
}