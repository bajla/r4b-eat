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

	public class AdminController : Controller
	{
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;



        public AdminController(ILogger<HomeController> logger, ApplicationDbContext db)
		{
            _logger = logger;
            _db = db;
        }

        public IActionResult AddUporabnikAdmin()
        {
            return View();
        }

        public IActionResult PredmetiUredi()
        {
            return View();
        }

        public IActionResult PredmetiAdd()
        {
            return View();
        }

        public IActionResult Profil()
        {
            return View();
        }

        public IActionResult Index()
        {

            if (HttpContext.Session.GetString("userId") != null)
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
            
        }

        public IActionResult Ucitelji()
        {
            if (HttpContext.Session.GetString("userId") != null)
            {

                var query = from uporabnik in _db.uporabniki
                            join poucevanje in _db.poucevanje
                            on uporabnik.id_uporabnika equals poucevanje.id_uporabnika into joinedPoucevanje
                            from leftJoinPoucevanje in joinedPoucevanje.DefaultIfEmpty()
                            join predmet in _db.predmeti
                            on leftJoinPoucevanje != null ? leftJoinPoucevanje.id_predmeta : (int?)null equals predmet.id_predmeta into joinedPredmet
                            from leftJoinPredmet in joinedPredmet.DefaultIfEmpty()

                            where uporabnik.pravice == "c"
                            orderby uporabnik.ime ascending
                            select new
                            {
                                
                                uporabnik.ime,
                                uporabnik.priimek,
                                leftJoinPredmet.predmet,
                                uporabnik.email

                            };



                var result = query.ToList();


                List<ucenciDisplayModel> model = new List<ucenciDisplayModel>();

                string lastIme = null;
                string lastPriimek = null;
                List<string> predmeti = new List<string>();
                string email = "";
                foreach (var i in result)
                {
                    if (lastIme == null)
                    {
                        lastIme = i.ime;
                        lastPriimek = i.priimek;
                        predmeti.Add(i.predmet);
                        email = i.email;
                    }

                    else if (lastIme == i.ime)
                    {
                        predmeti.Add(i.predmet);

                    }

                    else if (lastIme != i.ime)
                    {
                        model.Add(new ucenciDisplayModel { predmeti = new List<string>(predmeti), ime = lastIme, email = email, priimek = lastPriimek });
                        lastIme = i.ime;
                        lastPriimek = i.priimek;
                        email = i.email;
                        predmeti.Clear();
                        predmeti.Add(i.predmet);
                    }
                }

                model.Add(new ucenciDisplayModel { predmeti = new List<string>(predmeti), ime = lastIme, email = email, priimek = lastPriimek });


                return View(model);

            }

            return RedirectToAction("Index", "Home");
    }

        public IActionResult Predmeti()
        {

            if (HttpContext.Session.GetString("userId") != null)
            {
                var query = from poucevanje in _db.poucevanje
                            join uporabnik in _db.uporabniki on poucevanje.id_uporabnika equals uporabnik.id_uporabnika
                            join predmet in _db.predmeti on poucevanje.id_predmeta equals predmet.id_predmeta
                            where uporabnik.pravice == "c"
                            orderby predmet.predmet ascending, uporabnik.ime ascending
                            select new
                            {
                                uporabnik.ime,
                                uporabnik.priimek,
                                predmet.predmet,
                                predmet.opis

                            };



                var result = query.ToList();

                List<predmetiDisplayModel> model = new List<predmetiDisplayModel>();

                string lastPredmet = null;
                List<string> imena = new List<string>();
                string opis = "";
                foreach (var i in result)
                {
                    if (lastPredmet == null)
                    {
                        lastPredmet = i.predmet;
                        imena.Add(i.ime + " " + i.priimek);
                        opis = i.opis;
                    }

                    else if (lastPredmet == i.predmet)
                    {
                        imena.Add(i.ime + " " + i.priimek);

                    }

                    else if (lastPredmet != i.predmet)
                    {
                        model.Add(new predmetiDisplayModel { ime = new List<string>(imena), predmet = lastPredmet, opis = opis });
                        lastPredmet = i.predmet;
                        opis = i.opis;
                        imena.Clear();
                        imena.Add(i.ime + " " + i.priimek);
                    }
                }

                model.Add(new predmetiDisplayModel { ime = new List<string>(imena), predmet = lastPredmet, opis = opis });


                return View(model);
            }


            return RedirectToAction("Index", "Home");


        }

        public IActionResult Ucenci()
        {

            if (HttpContext.Session.GetString("userId") != null)
            {

                var query = from uporabnik in _db.uporabniki
                            join poucevanje in _db.poucevanje
                            on uporabnik.id_uporabnika equals poucevanje.id_uporabnika into joinedPoucevanje
                            from leftJoinPoucevanje in joinedPoucevanje.DefaultIfEmpty()
                            join predmet in _db.predmeti
                            on leftJoinPoucevanje != null ? leftJoinPoucevanje.id_predmeta : (int?)null equals predmet.id_predmeta into joinedPredmet
                            from leftJoinPredmet in joinedPredmet.DefaultIfEmpty()

                            where uporabnik.pravice == "u"
                            orderby uporabnik.ime ascending
                            select new
                            {
                                
                                uporabnik.ime,
                                uporabnik.priimek,
                                leftJoinPredmet.predmet,
                                uporabnik.email

                            };



                var result = query.ToList();


                List<ucenciDisplayModel> model = new List<ucenciDisplayModel>();

                string lastIme = null;
                string lastPriimek = null;
                List<string> predmeti = new List<string>();
                string email = "";
                foreach (var i in result)
                {
                    if (lastIme == null)
                    {
                        lastIme = i.ime;
                        lastPriimek = i.priimek;
                        predmeti.Add(i.predmet);
                        email = i.email;
                    }

                    else if (lastIme == i.ime)
                    {
                        predmeti.Add(i.predmet);

                    }

                    else if (lastIme != i.ime)
                    {
                        model.Add(new ucenciDisplayModel { predmeti = new List<string>(predmeti), ime = lastIme, email = email, priimek = lastPriimek });
                        lastIme = i.ime;
                        lastPriimek = i.priimek;
                        email = i.email;
                        predmeti.Clear();
                        predmeti.Add(i.predmet);
                    }
                }

                model.Add(new ucenciDisplayModel { predmeti = new List<string>(predmeti), ime = lastIme, email = email, priimek = lastPriimek });


                return View(model);

            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Naloge()
        {
            return View();
        }

        public IActionResult Gradiva()
        {
            return View();
        }

        public IActionResult AddUporabnik()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUporabnik(uporabnikiEntity uporabnik)
        {
            if (HttpContext.Session.GetString("userId") != null)
            {
                if (ModelState.IsValid)
                {
                    if (_db.uporabniki.Any(s => s.email == uporabnik.email) == false)
                    {

                        string gesloc = PasswordHelper.HashPassword(uporabnik.geslo);
                        uporabnik.geslo = gesloc;

                        _db.uporabniki.Add(uporabnik);
                        _db.SaveChanges();
                        return RedirectToAction("Ucenci");
                    }

                }
                return View();

            }

            return View();
        }


    }
}

