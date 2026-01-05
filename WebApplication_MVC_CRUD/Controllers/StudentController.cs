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

namespace WebApplication_MVC_CRUD.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        // Add this field to store the student API base URL
        private readonly string _studentUrl = "https://localhost:7246/api/student/";

        public StudentController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Index", "Login");

            var client = _clientFactory.CreateClient("Student");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("Student");

            if (!response.IsSuccessStatusCode)
                return View(new List<Student>());

            var students = await response.Content
                .ReadFromJsonAsync<List<Student>>();

            return View(students);
        }

       

        [HttpGet]
        public IActionResult Create()
        {
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Index", "Login");

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
            HttpResponseMessage response = await client.GetAsync(_studentUrl + id);
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
            HttpResponseMessage response = await client.PutAsync(_studentUrl + student.ID, content);
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
            HttpResponseMessage response = await client.GetAsync(_studentUrl + id);
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
            HttpResponseMessage response = await client.GetAsync(_studentUrl + id);
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
            HttpResponseMessage response = await client.DeleteAsync(_studentUrl + id);
            if (response.IsSuccessStatusCode)
            {
                TempData["delete_message"] = " student deleted";
                return RedirectToAction("Index");

            }
            return View();
        }


    } }

    // Fix: Define a DTO for the authentication response with Username and AccessToken properties.
    public class AuthResponse
    {
        public string? Username { get; set; }
        public string? AccessToken { get; set; }
    }

public class LoginRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}

public class LoginResponse
{
    [JsonProperty("Username")]
    public string? Username { get; set; }

    [JsonProperty("token")]
    public string? AccessToken { get; set; }
}





