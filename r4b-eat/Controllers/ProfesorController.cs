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
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


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
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


            return View();
        }


        public IActionResult Profil()
        {

            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


            return View();
        }

        public IActionResult DodajNalogo(int id)
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


            ViewBag.idPredmeta = id;
            return View();
        }

        [HttpPost]
        public IActionResult DodajNalogo(nalogeEntity naloga, IFormFile file)
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


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



        public IActionResult VseNaloge(int id)
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


            int idu = Convert.ToInt32(HttpContext.Session.GetString("userId"));

            var result = _db.naloge.Where(e => e.id_predmeta == id)
                                   .Where(r => r.id_uporabnika == idu).ToList();
                
            return View(result);
        }

        public IActionResult Naloga(int id)
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


            izdelavaNalogeDisplay display = new izdelavaNalogeDisplay();

            display.naloga = _db.naloge.Find(id);
            var query = from oddaja in _db.opravljene_Naloge
                        join uporabniki in _db.uporabniki
                        on oddaja.id_uporabnika equals uporabniki.id_uporabnika
                        join naloge in _db.naloge
                        on oddaja.id_naloge equals naloge.id_naloge
                        where oddaja.id_naloge == id
                        select new partialNalogeDisplay
                        {
                            id_oddaje = oddaja.id_opravljeno,
                            id_naloge = oddaja.id_naloge,
                            ime = uporabniki.ime,
                            priimek = uporabniki.priimek,
                            odziv = oddaja.odziv,
                            naloga = naloge.ime_naloge
                        };
            var result = query.ToList();


            int temp = 0;
            foreach(var i in result)
            {
                result[temp].datoteka = FileHelper.FindFile("wwwroot/Storage/Oddaja", i.id_oddaje.ToString());
                result[temp].imeDat = i.priimek + " " + i.ime + " - " + i.naloga + FileHelper.GetExtension(result[temp].datoteka);
                temp++;
            }

            ViewBag.file = FileHelper.FindFile("wwwroot/Storage/Naloge", id.ToString());

            display.oddaje = result;

            return View(display);
        }

        public IActionResult Gradiva()
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


            int id = Convert.ToInt32(HttpContext.Session.GetString("userId"));

            var query = from gradiva in _db.gradiva
                        join predmeti in _db.predmeti
                        on gradiva.id_predmeta equals predmeti.id_predmeta
                        where gradiva.id_uporabnika == id
                        select new gradivaDisplay
                        {
                            id_gradiva = gradiva.id_gradiva,
                            ime = gradiva.ime,
                            opis = gradiva.opis,
                            predmet = predmeti.predmet
                        };
            var result = query.ToList();

            int temp = 0;
            foreach(var i in result)
            {
                result[temp].ime_datoteke = FileHelper.FindFile("wwwroot/Storage/Gradiva", i.id_gradiva.ToString());
                temp++;
            }

            return View(result);
        }
        public IActionResult GradivaAdd()
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");

            int id = Convert.ToInt32(HttpContext.Session.GetString("userId"));


            var result = (from predmeti in _db.predmeti
                          join poucevanje in _db.poucevanje
                          on predmeti.id_predmeta equals poucevanje.id_predmeta
                          where poucevanje.id_uporabnika == id
                          select predmeti).ToList();

            ViewBag.predmeti = result;

            return View();
        }

        [HttpPost]
        public IActionResult GradivaAdd(gradivaEntity gradiva, IFormFile file, string predmet,string pomembno)
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


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
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


            return View();
        }

        public IActionResult GradivoOdstrani(int id)
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


            var gradivo =_db.gradiva.Find(id);

            _db.gradiva.Remove(gradivo);
            _db.SaveChanges();

            return RedirectToAction("Gradiva");
        }

        public IActionResult PotrdiNalogo(int id, int naloga)
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


            var oddaja = _db.opravljene_Naloge.Find(id);
            oddaja.odziv = 'd';
            _db.SaveChanges();

            return Redirect("/Profesor/Naloga?id="+naloga);
        }

        public IActionResult OvrziNalogo(int id, int naloga)
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "a") return RedirectToAction("Index", "Admin");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


            var oddaja = _db.opravljene_Naloge.Find(id);
            oddaja.odziv = 'n';
            _db.SaveChanges();

            return Redirect("/Profesor/Naloga?id=" + naloga);

        }

        public string CheckPriviliges()
        {
            return HttpContext.Session.GetString("userRights");
        }



    }
}

