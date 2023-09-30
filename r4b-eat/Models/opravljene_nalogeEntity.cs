using System;
using System.ComponentModel.DataAnnotations;

namespace r4b_eat.Models
{
	public class opravljene_nalogeEntity
    {
		[Key]
		public int id_upravljeno { get; set; }
		public string id_naloge { get; set; }
        public string id_uporabnika { get; set; }

        public uporabnikiEntity uporabniki { get; set; }
        public nalogeEntity naloge { get; set; }

    }
}

