using Dashboard.Models;
using System.Net.Http;

public class SalesService
{
    private readonly HttpClient _http;

    public SalesService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<SaleDTO>> GetSalesAsync()
    {
        return await _http.GetFromJsonAsync<List<SaleDTO>>("api/v1/Sales");
    }

    public async Task CreateSaleAsync(SaleDTO sale)
    {
        var response = await _http.PostAsJsonAsync("api/v1/Sales", sale);

        if (!response.IsSuccessStatusCode)
        {
            var errorDetails = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error {response.StatusCode}: {errorDetails}");
            throw new Exception($"Error al crear venta: {errorDetails}");
        }


    }

    public async Task<List<VentaHistorialDTO>> GetHistorialAsync()
    {
        return await _http.GetFromJsonAsync<List<VentaHistorialDTO>>("api/v1/Sales");
    }

    public async Task<byte[]> DescargarPdfFacturaAsync(int ventaId)
    {
        var response = await _http.GetAsync($"api/v1/Sales/{ventaId}/factura/pdf");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync();
    }

}
