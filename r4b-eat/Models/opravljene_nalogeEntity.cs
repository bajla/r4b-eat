using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace r4b_eat.Models
{
	public class opravljene_nalogeEntity
    {
		[Key]
		public int id_opravljeno { get; set; }
		public int id_naloge { get; set; }
        public int id_uporabnika { get; set; }
        public char odziv { get; set; }

        [ForeignKey("id_uporabnika")]
        public uporabnikiEntity uporabniki { get; set; }

        [ForeignKey("id_naloge")]
        public nalogeEntity naloge { get; set; }

    }
}

