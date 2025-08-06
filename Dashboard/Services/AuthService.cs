using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;


public class AuthService
{
    private readonly HttpClient _http;
    public string? Token { get; private set; }
    public bool IsAuthenticated => !string.IsNullOrEmpty(Token);

    private readonly IJSRuntime _jsRuntime;

    public event Action? OnAuthChanged;

    public bool IsInitialized { get; private set; } = false;


    public AuthService(HttpClient http, IJSRuntime jSRuntime)
    {
        _http = http;
        _jsRuntime = jSRuntime;
    }
    public async Task InitializeAsync()
    {
        if (string.IsNullOrEmpty(Token))
        {
            // Intenta cargar el token de localStorage
            Token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (!string.IsNullOrEmpty(Token))
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                System.Console.WriteLine("AuthService Initialized: Token loaded from localStorage.");

            }

            IsInitialized = true;
            NotifyAuthChanged();
        }
    }

    public async Task<bool> LoginAsync(string users, string password)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("https://localhost:7007/api/v1/Users/login", new { users, password });

            if (!response.IsSuccessStatusCode)
            {
                System.Console.WriteLine($"Login failed with status: {response.StatusCode}");
                var errorContent = await response.Content.ReadAsStringAsync();
                System.Console.WriteLine($"Error response: {errorContent}");
                return false;
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            Token = result?.Token;

            if (!string.IsNullOrEmpty(Token))
            {
                // ¡Clave! Guarda el token en localStorage después del login exitoso
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", Token);
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                System.Console.WriteLine("Login successful. Token saved to localStorage.");


                NotifyAuthChanged();

                return true;


            }

            System.Console.WriteLine("Login successful but token was empty.");
            return false;
        }
        catch (HttpRequestException httpEx)
        {
            System.Console.WriteLine($"Login HTTP error: {httpEx.Message}");
            return false;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Login generic error: {ex.Message}");
            return false;
        }
    }
    private void NotifyAuthChanged()
    {
        OnAuthChanged?.Invoke();
    }
    public async Task<bool> RegisterAsync(string users, string password)
    {
        var payload = new
        {
            Users = users,
            PassWord = password
        };

        var response = await _http.PostAsJsonAsync("https://localhost:7007/api/v1/Users/register", payload);
        return response.IsSuccessStatusCode;
    }

    public void Logout()
    {
        Token = null;
        _http.DefaultRequestHeaders.Authorization = null;
        

        NotifyAuthChanged();
    }

    
}

public class LoginResponse
{
    public string? Token { get; set; }
}



