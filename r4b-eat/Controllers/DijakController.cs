using System;
using Microsoft.AspNetCore.Mvc;
using r4b_eat.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using r4b_eat.Models;
using r4b_eat.Data;



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
            return View();
        }

        public IActionResult Nadzorna()
        {
            return View();
        }

        public IActionResult PredmetiOpis()
        {
            return View();
        }
    }
}

