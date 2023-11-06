using System;
using System.ComponentModel.DataAnnotations;

namespace r4b_eat.Models
{
	public class uporabnikiEntity
	{
		[Key]
		public int id_uporabnika { get; set; }
		public string ime { get; set; }
        public string priimek { get; set; }
		public DateTime starost { get; set; }
        public string email { get; set; }
        public string? pravice { get; set; }
		public string geslo { get; set; }

		public ICollection<poucevanjeEntity>? poucevanjeEntity { get; set; }
    }
}

