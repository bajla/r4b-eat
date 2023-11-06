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

    }
}

