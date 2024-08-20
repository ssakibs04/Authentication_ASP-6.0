# ASP.NET Core Authentication and Session Management Example
# Database First Approach

## Dashboard Action
### Step 1: Scaffold the Database Context and Models
```code
Scaffold-DbContext "server=SADMANSAKIB;database=Database Name;trusted_connection=true;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
```
### Step 2: Update Models After Adding Tables
```code
Scaffold-DbContext "server=SADMANSAKIB;database=Database Name;trusted_connection=true;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models 
```
### Step 3: Move Connection String to appsettings.json
```json
"ConnectionStrings": {
  "dbcs": "server=SADMANSAKIB;database=Database Name;trusted_connection=true;"
}
```
### Step 4: Configure DbContext in Program.cs
```csharp

var provider = builder.Services.BuildServiceProvider();
var config = provider.GetRequiredService<IConfiguration>();
builder.Services.AddDbContext<Model Context>(options =>
    options.UseSqlServer(config.GetConnectionString("dbcs")));
```
# Code First Approach
## Step 1: Create Models
Create your entity classes in the Models directory:

```csharp
public class Student
{
    public int StudentId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
```
## Step 2: Create DbContext Class
Add a DbContext class in the Models directory:

```csharp
public class Model Context : DbContext
{
    public Model Context(DbContextOptions<Mlodel Context> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
}
```
## Step 3: Configure DbContext in Program.cs
This step is identical to the Database First approach:

```csharp

builder.Services.AddDbContext<Model Context>(options =>
    options.UseSqlServer(config.GetConnectionString("dbcs")));
```
## Step 4: Apply Migrations
Run the following commands to create and apply the initial migration:

```code
Add-Migration InitialCreate
Update-Database
```
# Controller
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
# Login Actions

## GET: Login Action

```csharp
// GET: Login Action
public IActionResult Login()
{
    if (HttpContext.Session.GetString("userseason") != null)
    {

        return RedirectToAction("Dashboard");
    }
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


```
# Logout
```csharp
public IActionResult Logout()
{

    if (HttpContext.Session.GetString("userseason") != null)
    {

        HttpContext.Session.Remove("userseason");
        return RedirectToAction("Login");
    }
    return View();
}
```
# Signup
```csharp
  public IActionResult Register()
  {

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

