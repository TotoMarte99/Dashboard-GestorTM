using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class SaleItemDTO
    {

        [Required(ErrorMessage = "Debes seleccionar un producto.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un producto válido.")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int Cantidad { get; set; }
    }
}
