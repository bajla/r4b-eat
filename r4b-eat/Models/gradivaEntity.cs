using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace r4b_eat.Models
{
	public class gradivaEntity
	{
		[Key]
		public int id_gradiva { get; set; }
        public int id_predmeta { get; set; }
        public int id_uporabnika { get; set; }
        public string ime { get; set; }
        public string ime_datoteke { get; set; }
		public string? opis { get; set; }
        public char pomembno { get; set; }

        [ForeignKey("id_uporabnika")]
        public uporabnikiEntity uporabniki { get; set; }

        [ForeignKey("id_predmeta")]
        public predmetiEntity predmeti { get; set; }
    }
}

