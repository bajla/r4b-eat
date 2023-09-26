using System;
using System.ComponentModel.DataAnnotations;

namespace r4b_eat.Models
{
	public class nalogeEntity
	{
		[Key]
		public int id_naloge { get; set; }
		public int id_predmeta { get; set; }
        public int id_uporabnika { get; set; }
        public string ime_naloge { get; set; }
        public string navodilo_naloge { get; set; }

        public uporabnikiEntity uporabniki { get; set; }
        public predmetiEntity predmeti { get; set; }

    }
}

