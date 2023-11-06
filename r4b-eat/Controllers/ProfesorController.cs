using System;
using Microsoft.AspNetCore.Mvc;
using r4b_eat.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using r4b_eat.Models;
using r4b_eat.Data;
using r4b_eat.Services;



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
            int id = Convert.ToInt32(HttpContext.Session.GetString("userId"));

            var query = from poucevanje in _db.poucevanje
                        join predmeti in _db.predmeti
                        on poucevanje.id_predmeta equals predmeti.id_predmeta
                        where poucevanje.id_uporabnika == id
                        select new predmetiEntity
                        {
                            id_predmeta = predmeti.id_predmeta,
                            predmet = predmeti.predmet,
                            krajsava = predmeti.krajsava,
                            opis = predmeti.opis,
                            kljuc = predmeti.kljuc
                        };
           var result = query.ToList();


            return View(result);
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Profil()
        {
            return View();
        }

        public IActionResult DodajNalogo(int id)
        {

            ViewBag.idPredmeta = id;
            return View();
        }

        [HttpPost]
        public IActionResult DodajNalogo(nalogeEntity naloga, IFormFile file)
        {
            int id = Convert.ToInt32(HttpContext.Session.GetString("userId"));
            naloga.id_uporabnika = id;

            _db.naloge.Add(naloga);
            _db.SaveChanges();

            var query = from naloge in _db.naloge
                        where naloge.id_uporabnika == id
                        where naloge.id_predmeta == naloga.id_predmeta
                        select new
                        {
                            naloge.id_naloge
                        };

            var result = query.ToList();

            FileHelper.SaveNaloga(result.Last().id_naloge, file);

            return RedirectToAction("Predmeti");
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
        public IActionResult GradivaAdd(gradivaEntity gradiva, IFormFile file, string predmet,string pomembno)
        {
            if (pomembno == "pomembno") gradiva.pomembno = 'd';
            else gradiva.pomembno = 'n';

            int id = Convert.ToInt32(HttpContext.Session.GetString("userId"));

            gradiva.ime_datoteke = file.Name;
            gradiva.id_uporabnika = id;
            gradiva.id_predmeta = Convert.ToInt32(predmet);

            _db.gradiva.Add(gradiva);
            _db.SaveChanges();

            var query = from gradivo in _db.gradiva
                        where gradivo.id_predmeta == gradiva.id_predmeta
                        where gradivo.id_uporabnika == id
                        select new
                        {
                            gradivo.id_gradiva
                        };

            var result = query.ToList();

            FileHelper.SaveGradivo(result.Last().id_gradiva, file);

            return RedirectToAction("Gradiva");
        }

        public IActionResult SpecificnaNaloga()
        {
            return View();
        }
    }
}

