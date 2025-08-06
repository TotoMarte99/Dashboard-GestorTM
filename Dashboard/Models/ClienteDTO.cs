using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class ClienteDTO
    {
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string? Nombre { get; set; }

        [StringLength(100, ErrorMessage = "El apellido no puede exceder los 100 caracteres.")]
        public string? Apellido { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres.")]
        public string? Telefono { get; set; } 

        [StringLength(100, ErrorMessage = "El email no puede exceder los 100 caracteres.")]
        public string? Email { get; set; } 

        [StringLength(200, ErrorMessage = "La dirección no puede exceder los 200 caracteres.")]
        public string? Direccion { get; set; } 
    }
}
