namespace Dashboard.Models
{
    public class VentaHistorialDTO
    {
        public int Id { get; set; }
        public string ClienteNombre { get; set; }
        public DateTime Fecha { get; set; }
        public decimal? TotalVenta { get; set; }

        public string? ClienteTelefono { get; set; } // Usamos string? para permitir nulos
        public string? ClienteEmail { get; set; }   // Usamos string? para permitir nulos
        public string? ClienteDireccion { get; set; }

        public List<VentaItemHistorialDTO> Items { get; set; } = new();

    }
}
