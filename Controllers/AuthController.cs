using Microsoft.AspNetCore.Mvc;
using SimpleMovieDB.Models;

namespace SimpleMovieDB.Controllers;

public class AuthController : Controller
{
    // GET /Auth/Register
    public IActionResult Register() => View();

    // POST /Auth/Register
    [HttpPost]
    public IActionResult Register(RegisterModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Username))
        {
            ViewBag.Error = "Username is required.";
            return View(model);
        }
        if (string.IsNullOrWhiteSpace(model.Password))
        {
            ViewBag.Error = "Password is required.";
            return View(model);
        }
        if (UserStore.Users.Any(u => u.Username == model.Username))
        {
            ViewBag.Error = "Username already exists.";
            return View(model);
        }

        var user = new UserModel
        {
            Id       = UserStore.NextId++,
            Username = model.Username,
            Password = model.Password,
            Role     = model.Role == "Admin" ? "Admin" : "Regular"
        };
        UserStore.Users.Add(user);

        HttpContext.Session.SetString("UserId",   user.Id.ToString());
        HttpContext.Session.SetString("Username", user.Username);
        HttpContext.Session.SetString("Role",     user.Role);

        return RedirectToAction("Index", "Ssr");
    }

    // GET /Auth/Login
    public IActionResult Login() => View();

    // POST /Auth/Login
    [HttpPost]
    public IActionResult Login(LoginModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
        {
            ViewBag.Error = "Username and password are required.";
            return View(model);
        }

        var user = UserStore.Users.FirstOrDefault(u =>
            u.Username == model.Username && u.Password == model.Password);

        if (user == null)
        {
            ViewBag.Error = "Invalid username or password.";
            return View(model);
        }

        HttpContext.Session.SetString("UserId",   user.Id.ToString());
        HttpContext.Session.SetString("Username", user.Username);
        HttpContext.Session.SetString("Role",     user.Role);

        return RedirectToAction("Index", "Ssr");
    }

    // POST /Auth/Logout
    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
