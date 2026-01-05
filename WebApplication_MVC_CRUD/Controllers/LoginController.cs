using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
namespace WebApplication_MVC_CRUD.Controllers
{
   
    public class LoginController : Controller
    {
        private readonly HttpClient client;

        public LoginController(IHttpClientFactory factory)
        {
            client = factory.CreateClient();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest model)
        {
            // 🔴 BREAKPOINT HERE → WILL HIT
            var response = await client.PostAsJsonAsync(
                "https://localhost:7246/api/auth/login", model);

            if (!response.IsSuccessStatusCode)
                return View(model);

            // Read the response as a dynamic object to access properties like Username and AccessToken
            var authJson = await response.Content.ReadAsStringAsync();
            var auth = JsonConvert.DeserializeObject<LoginResponse>(authJson);

            // Fix: Check for null before dereferencing 'auth'
            if (auth == null || string.IsNullOrEmpty(auth.Username) || string.IsNullOrEmpty(auth.AccessToken))
            {
                ModelState.AddModelError("", "Invalid login");
                return View(model);
            }

           
            string Username = auth.Username;
            string accessToken = auth.AccessToken;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Username)
            };

            var identity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            HttpContext.Session.SetString("AccessToken", accessToken);

            return RedirectToAction("Index", "Student");
        }
    }
}
