using System;
using Microsoft.AspNetCore.Mvc;
using r4b_eat.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using r4b_eat.Models;
using r4b_eat.Data;
using r4b_eat.Services;
using Microsoft.EntityFrameworkCore;

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


        public IActionResult GradivaPredmet()
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");

            return View();
        }

        public IActionResult AddUporabnik()
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");

            addUserDisplay addUser = new addUserDisplay();
            addUser.predmeti = _db.predmeti.ToList();

            uporabnikiEntity user = new uporabnikiEntity();
            addUser.user = user;

            return View(addUser);
        }

        [HttpPost]
        public IActionResult AddUporabnik(addUserDisplay userPredmeti, string[] subjects)
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


            if (userPredmeti != null)
            {

                if (CheckIfEmailExist(userPredmeti.user.email))
                {
                    userPredmeti.user.geslo = PasswordHelper.HashPassword(userPredmeti.user.geslo);
                    _db.uporabniki.Add(userPredmeti.user);
                    _db.SaveChanges();

                    var query = from uporabniki in _db.uporabniki
                                where uporabniki.email == userPredmeti.user.email
                                select new
                                {
                                    uporabniki.id_uporabnika
                                };

                    var result = query.ToList();
                    int id = result[0].id_uporabnika;
                    foreach (var item in subjects)
                    {
                        _db.poucevanje.Add(new poucevanjeEntity { id_uporabnika = id, id_predmeta = Convert.ToInt32(item) });
                    }
                    _db.SaveChanges();

                }
                else
                {
                    ViewBag.error = "email ze obstaja";
                }
            }

            return RedirectToAction(userPredmeti.user.pravice == "c" ? "Ucitelji" : "Ucenci");
        }

        public IActionResult UrediUporabnik(int id)
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


            var user = _db.uporabniki.Find(id);

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

            addUserDisplay edituser = new addUserDisplay();
            edituser.user = user;
            edituser.predmeti = result;

            ViewBag.predmetos = _db.predmeti.ToList();

            return View(edituser);
        }

        [HttpPost]
        public IActionResult UrediUporabnik(addUserDisplay userPredmeti, string[] subjects)
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


            var user = _db.uporabniki.Find(userPredmeti.user.id_uporabnika);
            user.ime = userPredmeti.user.ime;
            user.priimek = userPredmeti.user.priimek;
            user.email = userPredmeti.user.email;
            if (userPredmeti.user.geslo != null) user.geslo = PasswordHelper.HashPassword(userPredmeti.user.geslo);
            user.starost = userPredmeti.user.starost;

            _db.SaveChanges();


            var query = from poucevanje in _db.poucevanje
                        join predmeti in _db.predmeti
                        on poucevanje.id_predmeta equals predmeti.id_predmeta
                        where poucevanje.id_uporabnika == userPredmeti.user.id_uporabnika
                        select new predmetiEntity
                        {
                            id_predmeta = predmeti.id_predmeta,
                            predmet = predmeti.predmet,
                            krajsava = predmeti.krajsava,
                            opis = predmeti.opis,
                            kljuc = predmeti.kljuc

                        };
            var result = query.ToList();


            List<int> oznaceniPredmeti = new List<int>();
            foreach (var i in subjects) oznaceniPredmeti.Add(Convert.ToInt32(i));
            List<int> shranjeniPredmeti = new List<int>();
            foreach (var i in result) shranjeniPredmeti.Add(i.id_predmeta);

            var dodaj = oznaceniPredmeti.Except(shranjeniPredmeti).ToList();
            foreach (var i in dodaj)
            {
                _db.poucevanje.Add(new poucevanjeEntity { id_uporabnika = userPredmeti.user.id_uporabnika, id_predmeta = i });
            }
            _db.SaveChanges();

            var zbrisi = shranjeniPredmeti.Except(oznaceniPredmeti).ToList();
            foreach (var i in zbrisi)
            {
                var query1 = from poucevanje in _db.poucevanje
                             where poucevanje.id_predmeta == i
                             where poucevanje.id_uporabnika == userPredmeti.user.id_uporabnika
                             select new poucevanjeEntity
                             {
                                 id_poucevanje = poucevanje.id_poucevanje,
                                 id_uporabnika = poucevanje.id_uporabnika,
                                 id_predmeta = poucevanje.id_predmeta
                             };
                var predmet = query1.ToList();
                _db.poucevanje.Remove(predmet.Last());
            }
            _db.SaveChanges();

            return RedirectToAction("Ucenci");
        }

        public IActionResult PredmetiUredi(int id)
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


            var predmetos = _db.predmeti.Find(id);
            return View(predmetos);
        }

        [HttpPost]
        public IActionResult PredmetiUredi(predmetiEntity predmetos, int id)
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


            if (predmetos != null)
            {

                var predmeto = _db.predmeti.Find(predmetos.id_predmeta);
                predmeto.kljuc = predmetos.kljuc;
                predmeto.opis = predmetos.opis;
                predmeto.krajsava = predmetos.krajsava;
                predmeto.predmet = predmetos.predmet;
                _db.SaveChanges();
            }
            return RedirectToAction("Predmeti");
        }

        public IActionResult PredmetiAdd()
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


            return View();
        }

        [HttpPost]
        public IActionResult PredmetiAdd(predmetiEntity predmetos)
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


            if (predmetos != null)
            {
                _db.predmeti.Add(predmetos);
                _db.SaveChanges();
            }

            return RedirectToAction("Predmeti");
        }


        public IActionResult Profil()
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


            return View();
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");

            return View();


        }

        public IActionResult Ucitelji()
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");



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
                            uporabnik.id_uporabnika,
                            uporabnik.ime,
                            uporabnik.priimek,
                            leftJoinPredmet.predmet,
                            uporabnik.email

                        };



            var result = query.ToList();


            List<ucenciDisplayModel> model = new List<ucenciDisplayModel>();

            int lastId = -1;
            string lastIme = null;
            string lastPriimek = null;
            List<string> predmeti = new List<string>();
            string email = "";
            foreach (var i in result)
            {
                if (lastIme == null)
                {
                    lastId = i.id_uporabnika;
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
                    model.Add(new ucenciDisplayModel { predmeti = new List<string>(predmeti), ime = lastIme, email = email, priimek = lastPriimek, id_uporabnika = lastId });
                    lastId = i.id_uporabnika;
                    lastIme = i.ime;
                    lastPriimek = i.priimek;
                    email = i.email;
                    predmeti.Clear();
                    predmeti.Add(i.predmet);
                }
            }

            model.Add(new ucenciDisplayModel { predmeti = new List<string>(predmeti), ime = lastIme, email = email, priimek = lastPriimek, id_uporabnika = lastId });


            return View(model);

        }
    


        public IActionResult Predmeti()
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");



            if (HttpContext.Session.GetString("userId") != null)
            {

                var query4 = from predmeti in _db.predmeti
                             join poucevanje in _db.poucevanje
                             on predmeti.id_predmeta equals poucevanje.id_predmeta into predmetiLeftJoin
                             from leftJoin in predmetiLeftJoin.DefaultIfEmpty()
                             join uporabniki in _db.uporabniki
                             on leftJoin.id_uporabnika equals uporabniki.id_uporabnika into uporabniki
                             from uporabnik in uporabniki.DefaultIfEmpty()


                             orderby predmeti.predmet ascending, uporabnik.ime ascending
                             select new
                             {
                                 uporabnik.ime,
                                 uporabnik.priimek,
                                 uporabnik.pravice,
                                 predmeti.predmet,
                                 predmeti.opis,
                                 predmeti.id_predmeta

                             };





                var result = query4.ToList();

                Console.WriteLine(_db.predmeti.ToList().Last().predmet);

                List<predmetiDisplayModel> model = new List<predmetiDisplayModel>();


                string lastPredmet = null;
                List<string> imena = new List<string>();
                string opis = "";
                int id = -1;
                foreach (var i in result)
                {
                    if (lastPredmet == null)
                    {
                        lastPredmet = i.predmet;
                        if (i.pravice == "c") imena.Add(i.ime + " " + i.priimek);
                        opis = i.opis;
                        id = i.id_predmeta;
                    }

                    else if (lastPredmet == i.predmet)
                    {
                        if (i.pravice == "c") imena.Add(i.ime + " " + i.priimek);

                    }

                    else if (lastPredmet != i.predmet)
                    {
                        model.Add(new predmetiDisplayModel { ime = new List<string>(imena), predmet = lastPredmet, opis = opis, id = id });
                        lastPredmet = i.predmet;
                        opis = i.opis;
                        imena.Clear();
                        id = i.id_predmeta;
                        if (i.pravice == "c") imena.Add(i.ime + " " + i.priimek);
                    }
                }

                model.Add(new predmetiDisplayModel { ime = new List<string>(imena), predmet = lastPredmet, opis = opis, id = id });


                return View(model);
            }


            return RedirectToAction("Index", "Home");


        }

        public IActionResult Ucenci()
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");



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
                                uporabnik.id_uporabnika,
                                uporabnik.ime,
                                uporabnik.priimek,
                                leftJoinPredmet.predmet,
                                uporabnik.email

                            };



                var result = query.ToList();


                List<ucenciDisplayModel> model = new List<ucenciDisplayModel>();

                int lastId = -1;
                string lastIme = null;
                string lastPriimek = null;
                List<string> predmeti = new List<string>();
                string email = "";
                foreach (var i in result)
                {
                    if (lastIme == null)
                    {
                        lastId = i.id_uporabnika;
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
                        model.Add(new ucenciDisplayModel { id_uporabnika = lastId, predmeti = new List<string>(predmeti), ime = lastIme, email = email, priimek = lastPriimek });
                        lastId = i.id_uporabnika;
                        lastIme = i.ime;
                        lastPriimek = i.priimek;
                        email = i.email;
                        predmeti.Clear();
                        predmeti.Add(i.predmet);
                    }
                }

                model.Add(new ucenciDisplayModel { id_uporabnika = lastId, predmeti = new List<string>(predmeti), ime = lastIme, email = email, priimek = lastPriimek });


                return View(model);

            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Naloge()
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


            return View();
        }

        public IActionResult Gradiva()
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


            string predmet = Request.Query["predmet"].ToString();
            if (predmet == null)
            {
                var predmeti = _db.gradiva.Select(p => p.predmeti).Distinct().ToList();
                return View(predmeti);
            }
            else
            {
                var gradiva = _db.gradiva.Where(p => p.predmeti.predmet == predmet);

                return View(gradiva);
            }

        }



        [HttpGet]
        public IActionResult Deleteuser(string page, int id)
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


            var user = _db.uporabniki.Find(id);

                if (user == null)
                {

                }
                else
                {
                    _db.uporabniki.Remove(user);
                    _db.SaveChanges();
                }





            return RedirectToAction(page);
        }

        public IActionResult DeletePredmet(int id)
        {
            if (HttpContext.Session.GetString("userId") == null) return RedirectToAction("Index", "Home");
            if (CheckPriviliges() == "c") return RedirectToAction("Index", "Profesor");
            else if (CheckPriviliges() == "u") return RedirectToAction("Index", "Dijak");


            var predmet = _db.predmeti.Find(id);


                if (predmet == null)
                {

                }
                else
                {
                    _db.predmeti.Remove(predmet);
                    _db.SaveChanges();
                }



            return RedirectToAction("Predmeti");

        }


        public bool CheckIfEmailExist(string email)
        {
            var query = from uporabniki in _db.uporabniki
                        where uporabniki.email == email
                        select new
                        {
                            uporabniki.id_uporabnika
                        };
            var result = query.ToList();

            if (result.Count() == 0) return false;
            else return true;
        }

        public string CheckPriviliges()
        {
            return HttpContext.Session.GetString("userRights");
        }

    }

}


