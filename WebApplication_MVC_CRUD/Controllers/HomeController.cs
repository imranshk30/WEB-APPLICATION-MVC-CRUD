using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using WebApplication_MVC_CRUD.Models;

namespace WebApplication_MVC_CRUD.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Student()
        {
            var token = HttpContext.Session.GetString("jwt");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login");


            var api = new ApiClient();
            api.SetJwt(token);


            var json = await api.GetStudentAsync();
            var model = JsonConvert.DeserializeObject<List<Student>>(json);


            return View(model);
        }
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //public async Task<ActionResult> Student()
        //{
        //    var token = HttpContext.Session.GetString("jwt");

        //    if (token == null)
        //        return RedirectToAction("Index", "Login");


        //    var api = new ApiClient();
        //    api.SetJwt(token);


        //    var json = await api.GetStudentAsync();
        //    var model = JsonConvert.DeserializeObject<List<Student>>(json);


        //    return View(model);
        //}
    }
}
