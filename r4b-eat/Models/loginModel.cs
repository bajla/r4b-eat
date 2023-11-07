using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace r4b_eat.Models
{
	public class loginModel
	{
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Geslo")]
        public string geslo { get; set; }
    }

    public class predmetiDisplayModel
    {
        public List<string> ime { get; set; }
        public string predmet { get; set; }
        public string opis { get; set; }
        public int id { get; set; }
    }

    public class ucenciDisplayModel
    {
        public List<string> predmeti { get; set; }
        public int id_uporabnika { get; set; }
        public string ime { get; set; }
        public string priimek { get; set; }
        public string email { get; set; }
        public string datum_rojstva { get; set; }
    }

    public class addUserDisplay
    {
        public uporabnikiEntity user { get; set; }
        public List<predmetiEntity> predmeti { get; set; }
    }

    public class predmetiOpisDisplay
    {
        public predmetiEntity predmet { get; set; }
        public List<gradivaEntity> gradiva { get; set; }
        public List<nalogeEntity> naloge { get; set; }
    }

    public class oddajaNalogeDisplay
    {
        public nalogeEntity naloga { get; set; }
        public string profesor { get; set; }
        public opravljene_nalogeEntity opravljena { get; set; }

    }

    public class partialNalogeDisplay
    {
        public int id_oddaje { get; set; }
        public int id_naloge { get; set; }
        public string ime { get; set; }
        public string priimek { get; set; }
        public string datoteka { get; set; }
        public char odziv { get; set; }
    }

    public class izdelavaNalogeDisplay
    {
        public nalogeEntity naloga { get; set; }
        public List<partialNalogeDisplay> oddaje { get; set; }
    }


    public class nadzornaDisplay
    {
        public List<gradivaEntity> gradiva { get; set; }
        public List<nalogeEntity> naloge { get; set; }
    }

    public class addPredmetDisplay
    {
        public List<predmetiEntity> predmeti { get; set; }
        public string kljuc { get; set; }
    }



}

