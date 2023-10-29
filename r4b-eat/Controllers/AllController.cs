﻿using System;
using Microsoft.AspNetCore.Mvc;
using r4b_eat.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using r4b_eat.Models;
using r4b_eat.Data;
using System.IO;
using System.Web;

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

        //!!! uredi ter spoliraj shranjevanje podatkov v pod. bazo 
        public IActionResult Profile()
        {
            if (HttpContext.Session.GetString("userId") != null)
            {
                int i = int.Parse(HttpContext.Session.GetString("userId"));
                var user = _db.uporabniki.Find(i);

                

                ViewBag.File = "/Storage/ProfilePics/" + i + ".png";

                return View(user);

            }
            else return RedirectToAction("Index", "Home");

            
        }

        [HttpPost]
        public IActionResult Profile(uporabnikiEntity uporabnik, IFormFile fileName)
        {
            uporabnik.id_uporabnika = int.Parse(HttpContext.Session.GetString("userId"));
            Console.WriteLine(uporabnik.id_uporabnika + " " + uporabnik.ime + " " + uporabnik.priimek + " " + uporabnik.starost + " " + uporabnik
                .pravice);
            _db.uporabniki.Update(uporabnik);
            _db.SaveChanges();

            
            using (var stream = new FileStream("wwwroot/Storage/ProfilePics/" + uporabnik.id_uporabnika+".png", FileMode.Create))
            {
                fileName.CopyTo(stream);
            }
            return View(uporabnik);
        }

        [HttpPost]
        public IActionResult Gradivo(gradivaEntity gradiva)
        {

            return View(gradiva);
        }

    }
}

