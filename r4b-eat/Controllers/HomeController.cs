using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using r4b_eat.Models;
using r4b_eat.Data;
using MySqlConnector;
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

        var query = from poucevanje in _db.poucevanje
                    join uporabnik in _db.uporabniki on poucevanje.id_uporabnika equals uporabnik.id_uporabnika
                    join predmet in _db.predmeti on poucevanje.id_predmeta equals predmet.id_predmeta
                    where uporabnik.pravice == "c"
                    select new
                    {
                        poucevanje.id_poucevanje,
                        uporabnik.ime,
                        uporabnik.priimek,
                        predmet.predmet
                    };

        var result = query.ToList();



        //Console.WriteLine(result[0].priimek);
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
                    HttpContext.Session.SetString("userRights", result[0].pravice.ToString());
                    
                    return RedirectToAction("Index", "Admin");
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
        uporabnik.pravice = "c";
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

