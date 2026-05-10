
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using WebApplication_MVC_CRUD.Models;

public class LoginController : Controller
{
    private readonly IHttpClientFactory _clientFactory;

    public LoginController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginRequest model)
    {
        //if (!ModelState.IsValid)
        //    return View(model);

        var client = _clientFactory.CreateClient("Student");

        var response = await client.PostAsJsonAsync("auth/token", new
        {
            Username = model.Username,
            Password = model.Password
        });
     
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Invalid username or password");
            return View(model);
        }
        //mvc reads the response and deserializes it into a LoginResponse object, which contains the access token.
        ////If the token is successfully retrieved, it is stored in the session and the user is authenticated using cookie authentication.
        /////Finally, the user is redirected to the Student index page.
        ///
        var tokenResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

        if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
        {
            ModelState.AddModelError("", "Failed to retrieve access token");
            return View(model);
        }

        //HttpContext.Session.SetString("AccessToken", tokenResponse.AccessToken);

        //return RedirectToAction("Index", "Student");
        // mapping AccessToken session to tokenResponse.AccessToken property forced by [JsonPropertyName]
        //MVC Stores JWT In Session

        HttpContext.Session.SetString("AccessToken", tokenResponse!.AccessToken);

        // Claims = user identity information for authentication(cookies).
        
        var claims = new List<Claim>
{
    new Claim(ClaimTypes.Name, model.Username!)
};

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        //ASP.NET uses ClaimsPrincipal as the logged-in user object.

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

        return RedirectToAction("Index", "Student");
    }
}