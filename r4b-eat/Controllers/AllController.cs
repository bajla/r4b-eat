using System;
using Microsoft.AspNetCore.Mvc;
using r4b_eat.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using r4b_eat.Models;
using r4b_eat.Data;
namespace r4b_eat.Controllers
{
	public class AllController : Controller
	{
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;


        public AllController(ILogger<HomeController> logger, ApplicationDbContext db)
		{
            _logger = logger;
            _db = db;
        }

        public IActionResult Profile()
        {
            if (HttpContext.Session.GetString("userId") != null)
            {
                var user = _db.uporabniki.Find(Convert.ToInt32(HttpContext.Session.GetString("userID")));

                return View(user);

            }
            else return RedirectToAction("Index", "Home");

            
        }

        [HttpPost]
        public IActionResult Profile(uporabnikiEntity uporabnik, IFormFile fileName)
        {
            _db.uporabniki.Update(uporabnik);

            using (var stream = new FileStream("~/slike/"+uporabnik.id_uporabnika+"png", FileMode.Create))
            {
                fileName.CopyToAsync(stream);
            }
            return View();
        }
    }
}

