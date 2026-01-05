using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


public class ApiClient
{
    private readonly HttpClient _client;


    public ApiClient()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://localhost:7246/");
    }


    public async Task<string> LoginAsync(string Username, string password)
    {
        var body = new { Username, password };
        var json = JsonConvert.SerializeObject(body);


        var response = await _client.PostAsync(
        "api/auth/login",
        new StringContent(json, Encoding.UTF8, "application/json")
        );


        return await response.Content.ReadAsStringAsync();
    }


    public void SetJwt(string token)
    {
        _client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", token);
    }


    public async Task<string> GetStudentAsync()
    {
        var response = await _client.GetAsync("api/Student");
        return await response.Content.ReadAsStringAsync();
    }
}

