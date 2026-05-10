using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using WebApplication_MVC_CRUD.Models;
using Newtonsoft.Json;

namespace WebApplication_MVC_CRUD.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        // Add this field to store the student API base URL
        // private readonly string _studentUrl = "https://localhost:7246/api/student/";

        // Since we have configured the HttpClient with a base address in Program.cs, we can just use the relative URL here.
        // This field will be used to specify the relative URL for the student API endpoints when making HTTP requests.
        // this is relative endpoint combined with the base address configured in Program.cs to form the full URL for API calls.
        //https://localhost:7246/api/student

        private readonly string _studentUrl = "student";
        // Inject the IHttpClientFactory to create HttpClient instances
        // The constructor takes an IHttpClientFactory as a parameter and assigns it to the _clientFactory field.
        // This allows the controller to create HttpClient instances when needed to make API calls.
        // The constructor is used to initialize the StudentController with the IHttpClientFactory dependency,
        // enabling it to create HttpClient instances for making API calls to the student API.
        // it is a common pattern in ASP.NET Core to use dependency injection for services like HttpClientFactory,
        // which promotes better testability and separation of concerns. HttpClientFactory is used to avoid socket exhaustion issues
        // that can arise from creating too many HttpClient instances and to manage the lifecycle of HttpClient instances more efficiently.
        // reuse connections and manage the lifecycle of HttpClient instances,
        // which can help improve performance and resource utilization in your application.
        public StudentController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        
        [HttpGet]
        // The Index action method is responsible for retrieving a list of students from the API and displaying them in the view.
        public async Task<IActionResult> Index()
        {
            // JWT TOKEN saved in AccessToken session and create token in MVC
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Index", "Login");

            // If the token is not found in the session, the user is redirected to the login page.
            // This ensures that only authenticated users can access the student list.

            // If the token is present, an HttpClient instance is created using the IHttpClientFactory,
            // and the Authorization header is set with the Bearer token.

            //Gets preconfigured client from DI container. The "Student" client is configured in Program.cs with the base address of the student API.
            // This allows the controller to make API calls to the student API using the configured HttpClient instance.
            var client = _clientFactory.CreateClient("Student");

            // The Authorization header is set to include the Bearer token, which is required for authenticated API requests.

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // An HTTP GET request is made to the student API endpoint to retrieve the list of students.
            //call api to get the list of students. The response is checked for success, and if successful,
            //the content is read and deserialized into a list of Student objects.

            var response = await client.GetAsync(_studentUrl);

            // If the API call is successful, the list of students is passed to the view for display. If the API call fails,
            // an empty list of students is passed to the view.
            if (!response.IsSuccessStatusCode)
                return View(new List<Student>());
            // If the API call is successful, the content of the response is read and deserialized into a list of Student objects.
            var students = await response.Content.ReadFromJsonAsync<List<Student>>();

            return View(students);
        }

       

        [HttpGet]
        public IActionResult Create()
        {
            // JWT TOKEN saved in AccessToken session and create token in MVC
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Index", "Login");
       //   

            var client = _clientFactory.CreateClient("Student");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            return View();
        }

       

        [HttpPost]
        public async Task<IActionResult> Create(Student student)
        {
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Index", "Login");

            var client = _clientFactory.CreateClient("Student");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

          //  var Client = _clientFactory.CreateClient("Student");
            string data = JsonConvert.SerializeObject(student);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(_studentUrl, content);

            //var token = HttpContext.Session.GetString("AccessToken");

            //if (string.IsNullOrEmpty(token))
            //    return RedirectToAction("Index", "Login");

            //var Client = _clientFactory.CreateClient("Student");
            //client.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue("Bearer", token);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Student Created Successfully";
                return RedirectToAction("Index");
            }
            return View(student);
        }
        //}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Index", "Login");

            var client = _clientFactory.CreateClient("Student");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            Student student = new Student();
           // var Client = _clientFactory.CreateClient("Student");
            HttpResponseMessage response = await client.GetAsync($"{_studentUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Student>(result);
                if (data != null)
                {
                    student = data;
                }
            }
            return View(student);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Student student)
        {
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Index", "Login");

            var client = _clientFactory.CreateClient("Student");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
           // var Client = _clientFactory.CreateClient("Student");
            string data = JsonConvert.SerializeObject(student);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync($"{_studentUrl}/{student.ID}", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["Updated_Message"] = "Student UPDATED Successfully";
                return RedirectToAction("Index");
            }
            return View(response);
        }
        [HttpGet]
        public async Task <IActionResult> Details(int id)
            
        {
            var token = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Index", "Login");
            var client = _clientFactory.CreateClient("Student");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            Student student = new Student();
           // var client = _clientFactory.CreateClient("Student");
            HttpResponseMessage response = await client.GetAsync($"{_studentUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Student>(result);
                if (data != null)
                {
                    student = data;
                }

            }
            return View(student);
        }
        /*
         
         */

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Index", "Login");

            var client = _clientFactory.CreateClient("Student");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            Student student = new Student();
           // var client = _clientFactory.CreateClient("Student");
            HttpResponseMessage response = await client.GetAsync($"{_studentUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Student>(result);
                if (data != null)
                {
                    student = data;
                }

            }
            return View(student);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteStudent(int id)
        {

            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Index", "Login");

            var client = _clientFactory.CreateClient("Student");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
           // var client = _clientFactory.CreateClient("Student");
            HttpResponseMessage response = await client.DeleteAsync($"{_studentUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["delete_message"] = " student deleted";
                return RedirectToAction("Index");

            }
            return View();
        }


    } }

    // Fix: Define a DTO for the authentication response with Username and AccessToken properties.
    //public class AuthResponse
    //{
    //    public string? Username { get; set; }
    //    public string? AccessToken { get; set; }
    //}

public class LoginRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}

//public class LoginResponse
//{
//    //[JsonProperty("Username")]
//    //public string? Username { get; set; }

//    [JsonProperty("access_token")]
//    public string? AccessToken { get; set; }
//}





