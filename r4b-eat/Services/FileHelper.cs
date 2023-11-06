using System;
using Microsoft.AspNetCore.Mvc;
using r4b_eat.Data;

namespace r4b_eat.Services
{
	public class FileHelper
	{
		public FileHelper()
		{

		}


        static public string GetProfilePicture()
        {
            int id = 5;

            if (System.IO.File.Exists("/Storage/ProfilePics/" + id + ".png"))
            {
                return "/Storage/ProfilePics/" + id + ".png";
            }
            else
            {
                return "/Storage/ProfilePics/default.png";
            }

        }



        static public string GetProfilePicture(int id)
		{
            if (System.IO.File.Exists("/Storage/ProfilePics/" + id + ".png"))
            {
                return "/Storage/ProfilePics/" + id + ".png";
            }
            else
            {
                return "/Storage/ProfilePics/default.png";
            }

        }

        static public string SaveGradivo(int id, IFormFile fileName)
        {
            string ext = Path.GetExtension(fileName.FileName);

            if (fileName != null)
            {
                using (var stream = new FileStream("wwwroot/Storage/Gradiva/" + id + "" + ext, FileMode.Create))
                {
                    fileName.CopyTo(stream);
                }
            }


            return "";
        }

        static public string SaveNaloga(int id, IFormFile fileName)
        {
            string ext = Path.GetExtension(fileName.FileName);

            if (fileName != null)
            {
                using (var stream = new FileStream("wwwroot/Storage/Naloge/" + id + "" + ext, FileMode.Create))
                {
                    fileName.CopyTo(stream);
                }
            }

            return "";
        }

    }
}

