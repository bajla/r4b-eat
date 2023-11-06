using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public DateTime rok_naloge { get; set; }

        [ForeignKey("id_uporabnika")]
        public uporabnikiEntity uporabniki { get; set; }

        [ForeignKey("id_predmeta")]
        public predmetiEntity predmeti { get; set; }

    }
}

