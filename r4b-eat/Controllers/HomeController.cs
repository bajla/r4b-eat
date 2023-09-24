using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using r4b_eat.Models;
using r4b_eat.Data;
using MySqlConnector;

namespace r4b_eat.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
    {
        _db = db;
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

    public IActionResult Login()
    {
        //List<uporabnikiEntity> uporabniki = new List<uporabnikiEntity>();
        //uporabniki = _db.uporabniki.ToList();


        return View();
    }



    public IActionResult Registration()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Registration(uporabnikiEntity uporabnik)
    {
        uporabnik.pravice = "u";
        //if (ModelState.IsValid)
        //{ 
            _db.uporabniki.Add(uporabnik);
            _db.SaveChanges();
            return RedirectToAction("Login");
        //}
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

