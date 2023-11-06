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

	public class DijakController : Controller
	{
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;



        public DijakController(ILogger<HomeController> logger, ApplicationDbContext db)
		{
            _logger = logger;
            _db = db;
        }

        public IActionResult PredmetiAdd()
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

        public IActionResult Nadzorna()
        {
            return View();
        }

        public IActionResult PredmetiOpis(int id)
        {
            var predmet = _db.predmeti.Find(id);

            var gradiva = _db.gradiva.Where(e => e.id_predmeta == id).ToList();

            var naloge = _db.naloge.Where(e => e.id_predmeta == id).ToList();

            predmetiOpisDisplay display = new predmetiOpisDisplay();

            display.predmet = predmet;
            display.gradiva = gradiva;
            display.naloge = naloge;

            return View(display);
        }

        public IActionResult Gradiva(int id)
        {
            var gradivo = _db.gradiva.Find(id);

            ViewBag.file = FileHelper.FindFile("wwwroot/Storage/Gradiva", id.ToString());

            return View(gradivo);
        }

        public IActionResult Naloge()
        {
            return View();
        }

        public IActionResult OddajaNaloge(int id)
        {
            int idu = Convert.ToInt32(HttpContext.Session.GetString("userId"));

            var naloga = _db.naloge.Find(id);

            var query = from uporabniki in _db.uporabniki
                        join naloge in _db.naloge
                        on uporabniki.id_uporabnika equals naloge.id_uporabnika
                        where naloge.id_naloge == id
                        select new
                        {
                            uporabniki.ime
                        };
            var result = query.ToList();

            oddajaNalogeDisplay oddaja = new oddajaNalogeDisplay();

            oddaja.naloga = naloga;
            oddaja.profesor = result.Last().ime;

            ViewBag.file = FileHelper.FindFile("wwwroot/Storage/Naloge", id.ToString());

            var query1 = from oddaja1 in _db.opravljene_Naloge
                         where oddaja1.id_naloge == id
                         where oddaja1.id_uporabnika == idu
                         select new
                         {
                             oddaja1.id_opravljeno,
                             oddaja1.odziv
                         };
            var result1 = query1.ToList();

            if (result1.Count() != 0)
            {
                ViewBag.locked = "true";
                ViewBag.odziv = result1.Last().odziv;
            }
            else ViewBag.locked = "false";


            return View(oddaja);
        }

        [HttpPost]
        public IActionResult OddajaNaloge(oddajaNalogeDisplay oddaja, IFormFile file)
        {
            int id = Convert.ToInt32(HttpContext.Session.GetString("userId"));

            oddaja.opravljena.id_uporabnika = id;

            _db.opravljene_Naloge.Add(oddaja.opravljena);
            _db.SaveChanges();

            var query = from opravljena in _db.opravljene_Naloge
                        where opravljena.id_uporabnika == id
                        where opravljena.id_naloge == oddaja.opravljena.id_naloge
                        select new
                        {
                            opravljena.id_opravljeno
                        };

            var result = query.ToList();

            FileHelper.SaveOddaja(id,file);


            return RedirectToAction("Predmeti");
        }

    }
}

