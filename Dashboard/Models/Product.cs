using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class Product
    {
        public int id { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "El modelo es obligatorio")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "El tipo es obligatorio")]
        public string Tipo { get; set; }

        //[Required(ErrorMessage = "El precio es obligatorio")] // Se recomienda Required para decimales
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "El precio debe ser un número positivo")]

        public decimal Precio { get; set; }

        public decimal PrecioVenta { get; set; }
        public int Stock { get; set; }
        public string StockCritico { get; set; }

        public DateTime FechaIngreso { get; set; } = DateTime.Now;



    }
}
