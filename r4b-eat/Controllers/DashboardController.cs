using System;
using r4b_eat.Data;

namespace r4b_eat.Controllers
{
	public class DashboardController
	{
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;



        public DashboardController(ILogger<HomeController> logger, ApplicationDbContext db)
		{
            _logger = logger;
            _db = db;
        }
	}
}

