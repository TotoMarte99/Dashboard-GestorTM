using Dashboard.Models;
using System.Net.Http.Headers;

namespace Dashboard.Services
{
    public class ClientService
    {
        private readonly HttpClient _http;
        private readonly AuthService _auth; // Mantenemos el AuthService inyectado

        public ClientService(HttpClient http, AuthService auth)
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
                Console.WriteLine("Sesion Iniciada");
            }
            else
            {
                // Asegurarse de que no haya cabecera de autorización si no está autenticado
                Console.WriteLine("ProductService: No se estableció cabecera de Autorización (no autenticado o token vacío).");
            }
        }

        public async Task<List<ClienteDTO>> GetClientAsync()
        {
            AddAuthorizationHeader(); // Llama al método auxiliar antes de la petición
            try
            {
                return await _http.GetFromJsonAsync<List<ClienteDTO>>("api/v1/Client");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"ProductService GetProductsAsync error: {ex.Message}");
                // Puedes manejar errores específicos de HTTP aquí si es necesario
                throw; // Relanza la excepción para que sea manejada en el componente si es necesario
            }
        }

        public async Task AddClientAsync(ClienteDTO cliente)
        {
            if (!_auth.IsAuthenticated) throw new UnauthorizedAccessException();
            AddAuthorizationHeader(); // Llama al método auxiliar antes de la petición
            await _http.PostAsJsonAsync("api/v1/Client", cliente);
        }

        public async Task UpdateClientAsync(ClienteDTO cliente)
        {
            if (!_auth.IsAuthenticated) throw new UnauthorizedAccessException();
            AddAuthorizationHeader(); // Llama al método auxiliar antes de la petición
            await _http.PutAsJsonAsync($"api/v1/Client/{cliente.Id}", cliente);
        }

        public async Task DeleteClientAsync(int id)
        {
            if (!_auth.IsAuthenticated) throw new UnauthorizedAccessException();
            AddAuthorizationHeader(); // Llama al método auxiliar antes de la petición
            await _http.DeleteAsync($"api/v1/Client/{id}");
        }
    }
}
