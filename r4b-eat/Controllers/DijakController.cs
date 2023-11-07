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
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");


            int id = Convert.ToInt32(HttpContext.Session.GetString("userId"));

            addPredmetDisplay display = new addPredmetDisplay();

            var query = from 
                         predmeti in _db.predmeti
                        
               
                        select new predmetiEntity
                        {
                            id_predmeta = predmeti.id_predmeta,
                            predmet = predmeti.predmet,
                            krajsava = predmeti.krajsava,
                            opis = predmeti.opis,
                            kljuc = predmeti.kljuc

                        };
            var result = query.Distinct().ToList();

            var query1 = from poucevanje in _db.poucevanje
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
            var result1 = query1.ToList();

            var razlika = result.Except(result1).ToList();

            display.predmeti = razlika;

            return View(display);
        }

        [HttpPost]
        public IActionResult PredmetiAdd(addPredmetDisplay display, string predmet)
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");


            int id = Convert.ToInt32(HttpContext.Session.GetString("userId"));

            var predmetos = _db.predmeti.Find(Convert.ToInt32(predmet));

            if (display.kljuc == predmetos.kljuc)
            {
                _db.poucevanje.Add(new poucevanjeEntity { id_predmeta = Convert.ToInt32(predmet) , id_uporabnika = id});
            }

            return RedirectToAction("Predmeti");
        }


        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");


            return View();
        }


        public IActionResult Profil()
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");


            return View();
        }

        public IActionResult Predmeti()
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");


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
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");


            int idu = Convert.ToInt32(HttpContext.Session.GetString("userId"));

            nadzornaDisplay display = new nadzornaDisplay();

            var query = from poucevanje in _db.poucevanje
                        join gradiva in _db.gradiva
                        on poucevanje.id_predmeta equals gradiva.id_predmeta
                        where poucevanje.id_uporabnika == idu
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
            display.gradiva = result;

            var query1 = from poucevanje in _db.poucevanje
                        join naloge in _db.naloge
                        on poucevanje.id_predmeta equals naloge.id_predmeta
                        where poucevanje.id_uporabnika == idu
                        select new nalogeEntity
                        {
                            id_naloge = naloge.id_naloge,
                            id_predmeta = naloge.id_predmeta,
                            id_uporabnika = naloge.id_uporabnika,
                            ime_naloge = naloge.ime_naloge,
                            navodilo_naloge = naloge.navodilo_naloge,
                            rok_naloge = naloge.rok_naloge

                        };

            var result1 = query1.ToList();

            display.naloge = result1;

            return View(display);
        }

        public IActionResult PredmetiOpis(int id)
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");


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
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");


            var gradivo = _db.gradiva.Find(id);

            ViewBag.file = FileHelper.FindFile("wwwroot/Storage/Gradiva", id.ToString());

            return View(gradivo);
        }

        public IActionResult Naloge()
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");


            return View();
        }

        public IActionResult OddajaNaloge(int id)
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");


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
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");


            int id = Convert.ToInt32(HttpContext.Session.GetString("userId"));

            

            oddaja.opravljena.id_uporabnika = id;

            if (oddaja.opravljena.id_opravljeno != 0)
            {
                FileHelper.SaveOddaja(oddaja.opravljena.id_opravljeno, file);

            }
            else
            {

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

                FileHelper.SaveOddaja(result.Last().id_opravljeno, file);

            }
            return RedirectToAction("Predmeti");
        }

        public string CheckPriviliges()
        {
            return HttpContext.Session.GetString("userRights");
        }


    }
}

