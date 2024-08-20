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
