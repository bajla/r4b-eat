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
    }

    public class ucenciDisplayModel
    {
        public List<string> predmeti { get; set; }
        public string ime { get; set; }
        public string priimek { get; set; }
        public string email { get; set; }
        public string datum_rojstva { get; set; }
    }


}

