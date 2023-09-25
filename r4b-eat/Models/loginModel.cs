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
}

