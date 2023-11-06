using System;
using Microsoft.AspNetCore.Mvc;
using r4b_eat.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using r4b_eat.Models;
using r4b_eat.Data;



namespace r4b_eat.Controllers
{

	public class ProfesorController : Controller
	{
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;



        public ProfesorController(ILogger<HomeController> logger, ApplicationDbContext db)
		{
            _logger = logger;
            _db = db;
        }

        public IActionResult Predmeti()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Profil()
        {
            return View();
        }

        public IActionResult Predmet()
        {
            return View();
        }

        public IActionResult VseNaloge()
        {
            return View();
        }

        public IActionResult VsiDijaki()
        {
            return View();
        }

        public IActionResult Gradiva()
        {

            int id = Convert.ToInt32(HttpContext.Session.GetString("userId"));

            var query = from gradiva in _db.gradiva
                        where gradiva.id_uporabnika == id
                        select new gradivaEntity
                        {
                            id_gradiva = gradiva.id_gradiva,
                            id_uporabnika = gradiva.id_uporabnika,
                            id_predmeta = gradiva.id_predmeta,
                            ime = gradiva.ime,
                            opis = gradiva.opis,
                            pomembno = gradiva.pomembno
                        };
            var result = query.ToList();



            return View(result);
        }
        public IActionResult GradivaAdd()
        {
            ViewBag.predmeti = _db.predmeti.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult GradivaAdd(gradivaEntity gradiva)
        {
            _db.gradiva.Add(gradiva);
            _db.SaveChanges();

            return RedirectToAction("Gradiva");
        }

        public IActionResult SpecificnaNaloga()
        {
            return View();
        }
    }
}

