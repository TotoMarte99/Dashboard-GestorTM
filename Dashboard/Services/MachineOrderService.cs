using Dashboard.Components.Pages.OrdersMachines;
using Dashboard.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Dashboard.Services
{
    public class MachineOrderService
    {
        private readonly HttpClient _http;
        private readonly AuthService _auth;
    
    public MachineOrderService(HttpClient http, AuthService auth)
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

        public async Task<List<MachinesOrder>> GetOrderAsync()
        {
            AddAuthorizationHeader();
            try
            {
                // 2. Cambia el tipo genérico de GetFromJsonAsync
                return await _http.GetFromJsonAsync<List<MachinesOrder>>("api/v1/MaOrders");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"MachineOrderService GetOrdersAsync error: {ex.Message}");
                return null; // O relanza la excepción si lo prefieres
            }
        }

        public async Task<MachinesOrder?> GetOrderByIdAsync(int id)
        {
            AddAuthorizationHeader();
            return await _http.GetFromJsonAsync<MachinesOrder>($"api/v1/MaOrders/{id}");
        }

        public async Task AddOrderAsync(MachinesOrder orden)
        {
            if (!_auth.IsAuthenticated) throw new UnauthorizedAccessException();
            AddAuthorizationHeader(); // Llama al método auxiliar antes de la petición
            await _http.PostAsJsonAsync("api/v1/MaOrders/ordenes", orden);
        }

        public async Task UpdateOrderAsync(int id, MaOrderUpdateDTO dto)
        {
            var response = await _http.PutAsJsonAsync($"api/v1/MaOrders/{id}", dto);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error al actualizar orden: {response.StatusCode}");
        }

        public async Task<byte[]?> GetOrdenPdfAsync(int ordenId)
        {
            if (!_auth.IsAuthenticated) throw new UnauthorizedAccessException();
            AddAuthorizationHeader();
            var response = await _http.GetAsync($"api/v1/MaOrders/orden/pdf/{ordenId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }

            return null;
        }
    }
}
