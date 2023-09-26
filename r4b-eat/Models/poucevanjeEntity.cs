using System;
using System.ComponentModel.DataAnnotations;

namespace r4b_eat.Models
{
	public class poucevanjeEntity
	{
		[Key]
		public int id_poucevanje { get; set; }
		public int id_uporabnika { get; set; }
        public int id_predmeta { get; set; }

		public uporabnikiEntity uporabniki { get; set; }
		public predmetiEntity predmeti { get; set; }
    }
}

