using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using r4b_eat.Models;
using r4b_eat.Data;
using MySqlConnector;
using System.Security.Cryptography;
using r4b_eat.Services;
using Microsoft.AspNetCore.Http;


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
        
        return View();
    }

    [HttpPost]
    public IActionResult Login(loginModel login)
    {
        if (ModelState.IsValid)
        {
            var result = _db.uporabniki.Where(s => s.email == login.email).ToList();
            
            if(result != null)
            {
                if (PasswordHelper.VerifyPassword(login.geslo, result[0].geslo))
                {
                    HttpContext.Session.SetString("userId", result[0].id_uporabnika.ToString());
                    return RedirectToAction("Index");
                }
                
            }
        }

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
        if (ModelState.IsValid)
        {
            if (_db.uporabniki.Any(s=> s.email == uporabnik.email) == false)
            {
                
                string gesloc = PasswordHelper.HashPassword(uporabnik.geslo);
                uporabnik.geslo = gesloc;

                _db.uporabniki.Add(uporabnik);
                _db.SaveChanges();
                return RedirectToAction("Login");
            }

        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

