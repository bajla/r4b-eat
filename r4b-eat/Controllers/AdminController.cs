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

        public IActionResult Predmeti()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


    }
}

