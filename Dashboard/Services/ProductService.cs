// ProductService.cs
using Dashboard.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class ProductService
{
    private readonly HttpClient _http;
    private readonly AuthService _auth; // Mantenemos el AuthService inyectado

    public ProductService(HttpClient http, AuthService auth)
    {
        _http = http;
        _auth = auth;
    }

    // NUEVO MÉTODO AUXILIAR para añadir la cabecera de autorización
    private void AddAuthorizationHeader()
    {
        // Limpiamos la cabecera de Autorización por si ya existía para evitar duplicados
        // y para asegurar que siempre se usa el token actual.
        _http.DefaultRequestHeaders.Authorization = null;

        if (_auth.IsAuthenticated && !string.IsNullOrEmpty(_auth.Token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _auth.Token);
            Console.WriteLine("ProductService: Cabecera de Autorización establecida explícitamente para esta petición.");
        }
        else
        {
            // Asegurarse de que no haya cabecera de autorización si no está autenticado
            Console.WriteLine("ProductService: No se estableció cabecera de Autorización (no autenticado o token vacío).");
        }
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        AddAuthorizationHeader(); // Llama al método auxiliar antes de la petición
        try
        {
            return await _http.GetFromJsonAsync<List<Product>>("api/v1/Machines");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"ProductService GetProductsAsync error: {ex.Message}");
            // Puedes manejar errores específicos de HTTP aquí si es necesario
            throw; // Relanza la excepción para que sea manejada en el componente si es necesario
        }
    }

    public async Task AddProductAsync(Product product)
    {
        if (!_auth.IsAuthenticated) throw new UnauthorizedAccessException();
        AddAuthorizationHeader(); // Llama al método auxiliar antes de la petición
        await _http.PostAsJsonAsync("api/v1/Machines", product);
    }

    public async Task UpdateProductAsync(Product product)
    {
        if (!_auth.IsAuthenticated) throw new UnauthorizedAccessException();
        AddAuthorizationHeader(); // Llama al método auxiliar antes de la petición
        await _http.PutAsJsonAsync($"api/v1/Machines/{product.id}", product);
    }

    public async Task DeleteProductAsync(int id)
    {
        if (!_auth.IsAuthenticated) throw new UnauthorizedAccessException();
        AddAuthorizationHeader(); // Llama al método auxiliar antes de la petición
        await _http.DeleteAsync($"api/v1/Machines/{id}");
    }
}