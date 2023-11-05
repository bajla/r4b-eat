using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace r4b_eat.Models
{
	public class poucevanjeEntity
	{
		[Key]
		public int id_poucevanje { get; set; }
		public int id_uporabnika { get; set; }
        public int id_predmeta { get; set; }

		[ForeignKey("id_uporabnika")]
		public uporabnikiEntity uporabnikiEntity { get; set; }

        [ForeignKey("id_predmeta")]
        public predmetiEntity predmetiEntity { get; set; }
    }
}

