using LoginForm_ASP_6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;

namespace LoginForm_ASP_6.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly StudentDBContext context;

        public HomeController(StudentDBContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        //Admin Dashbaord
        public async Task<IActionResult> AdminDashboard()
{
    
    string? userEmail = HttpContext.Session.GetString("userseason");// Get the user's email from the session

   
    if (userEmail != null && userEmail == "admin@gmail.com") 
    {
        // Retrieve data for the admin
        var students = await context.Students.ToListAsync();
        return View(students);
    }
    else
    {
       
        return RedirectToAction("Login");
    }
}


        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("userseason") != null)
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

      //login post



        [HttpPost]

      
        public IActionResult Login(Student user)
        {
            // Check if the user's email and password match a record in the database
            var myUser = context.Students.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);

            if (myUser != null)
            {
                // Store the user's email in the session
                HttpContext.Session.SetString("userseason", myUser.Email);

           
                if (myUser.Email == "admin@gmail.com")
                {
                    return RedirectToAction("AdminDashboard", "Home");
                }
                else
                {
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            else
            {
                // If login fails, show an error message
                ViewBag.message = "Login Failed";
                return View(user);
            }
        }

        //logout
        //logout
        public IActionResult Logout()
        {

            if (HttpContext.Session.GetString("userseason") != null)
            {

                HttpContext.Session.Remove("userseason");
                return RedirectToAction("Login","Home");
            }
            return View();
        }



        //Register
        public IActionResult Register()
        {
            List<SelectListItem> Gender = new()
            {
                new SelectListItem {Value="Male",Text="Male"},
                new SelectListItem{Value="Female",Text="Female"},
               new SelectListItem{Value="Others",Text="Others"}
            };
            ViewBag.Gender = Gender;    

            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Register(Student std)
        {
            if (ModelState.IsValid) { 
            await context.Students.AddAsync(std);
                await context.SaveChangesAsync();
                TempData["Success"] = "Registerd Successfully";
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
