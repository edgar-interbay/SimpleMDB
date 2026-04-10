using Microsoft.AspNetCore.Mvc;

namespace SimpleMovieDB.Controllers;

public class HomeController : Controller
{

    public IActionResult Index()
    {
        return View();
    }


    public IActionResult Actors()
    {
        return View();
    }


    public IActionResult Movies()
    {
        return View();
    }


    public IActionResult Users()
    {
        return View();
    }


    public IActionResult CreateMovie()
    {
        return View();
    }
}