using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class SaleDTO
    {

        public string ClienteNombre { get; set; } = string.Empty;
       // [Required(ErrorMessage = "Los datos del cliente son requeridos.")]
        public ClienteDTO Cliente { get; set; } = new ClienteDTO();

        //  public DateTime Fecha { get; set; } = DateTime.Now;
        public List<SaleItemDTO> Items { get; set; } = new();
    }
}
