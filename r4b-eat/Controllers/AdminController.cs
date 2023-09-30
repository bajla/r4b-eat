    using System;
using Microsoft.AspNetCore.Mvc;
using r4b_eat.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using r4b_eat.Models;
using r4b_eat.Data;

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

        public IActionResult PredmetiAdd()
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
            return View();
        }

        public IActionResult Predmeti()
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
            foreach(var i in result)
            {
                if (lastPredmet == null)
                {
                    lastPredmet = i.predmet;
                    imena.Add(i.ime + " " + i.priimek);
                    opis = i.opis;
                }

                else if(lastPredmet == i.predmet)
                {
                    imena.Add(i.ime + " " + i.priimek);

                }

                else if(lastPredmet != i.predmet)
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

        public IActionResult Ucenci()
        {
            return View();
        }

        public IActionResult Naloge()
        {
            return View();
        }

        public IActionResult Gradiva()
        {
            return View();
        }

    }
}

