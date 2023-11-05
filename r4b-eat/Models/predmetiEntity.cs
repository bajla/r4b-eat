using System;
using System.ComponentModel.DataAnnotations;

namespace r4b_eat.Models
{
	public class predmetiEntity
	{
            [Key]
            public int id_predmeta { get; set; }
            public string predmet { get; set; }
            public string krajsava { get; set; }
            public string? opis { get; set; }
            public string? kljuc { get; set; }

        public ICollection<poucevanjeEntity> poucevanjeEntity { get; set; }

    }
}

