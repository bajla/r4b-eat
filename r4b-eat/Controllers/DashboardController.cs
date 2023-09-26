using System;
using r4b_eat.Data;

namespace r4b_eat.Controllers
{
<<<<<<< Updated upstream:r4b-eat/Controllers/DashboardController.cs
	public class DashboardController
=======
	public class AdminController : Controller
>>>>>>> Stashed changes:r4b-eat/Controllers/AdminController.cs
	{
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;



        public AdminController(ILogger<HomeController> logger, ApplicationDbContext db)
		{
            _logger = logger;
            _db = db;
        }
	}
}

