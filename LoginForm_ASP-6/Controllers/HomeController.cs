using LoginForm_ASP_6.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LoginForm_ASP_6.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly loginDBContext context;

        public HomeController(loginDBContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        //Dashbaord
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetString("userseason")!=null)
            {

                ViewBag.Myseason = HttpContext.Session.GetString("userseason");

            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        //login
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("userseason") != null)
            {

                return RedirectToAction("Dashboard");
            }
            return View();
        }
        [HttpPost]
        //login post
        public IActionResult Login(Student user)
        {
            var myUser= context.Students.Where(x=>x.Email== user.Email && x.Password==user.Password).FirstOrDefault();

            if (myUser != null) { 
                HttpContext.Session.SetString("userseason",myUser.Email);
                return RedirectToAction("Dashboard");

            }
            else
            {
                ViewBag.message = "Login Failed";
            }
            return View(myUser);
        }
        public IActionResult Logout()
        {

            if (HttpContext.Session.GetString("userseason") != null)
            {

                HttpContext.Session.Remove("userseason");
                return RedirectToAction("Login");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
