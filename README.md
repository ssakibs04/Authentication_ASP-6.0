# ASP.NET Core Authentication and Session Management Example

This guide demonstrates how to implement basic authentication and session management in an ASP.NET Core application.

## Dashboard Action


```csharp
// Dashboard Action
public IActionResult Dashboard()
{
    // Check if the user session exists
    if (HttpContext.Session.GetString("userseason") != null)
    {
        // Pass session data to the View
        ViewBag.Myseason = HttpContext.Session.GetString("userseason");
    }
    else
    {
        // Redirect to Login if session is null
        return RedirectToAction("Login");
    }
    
    return View();
}
```
## GET: Login Action

```csharp
// GET: Login Action
public IActionResult Login()
{
    return View();
}
```
## POST: Login Action

```csharp
[HttpPost]
// POST: Login Action
public IActionResult Login(Student user)
{
    // Authenticate user
    var myUser = context.Students
                        .Where(x => x.Email == user.Email && x.Password == user.Password)
                        .FirstOrDefault();

    if (myUser != null)
    {
        // Set session and redirect to Dashboard
        HttpContext.Session.SetString("userseason", myUser.Email);
        return RedirectToAction("Dashboard");
    }
    else
    {
        // Display error message
        ViewBag.message = "Login Failed";
    }

    return View(myUser);
}
