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

        static public string SaveOddaja(int id, IFormFile fileName)
        {
            string ext = Path.GetExtension(fileName.FileName);

            if (fileName != null)
            {
                using (var stream = new FileStream("wwwroot/Storage/Oddaja/" + id + "" + ext, FileMode.Create))
                {
                    fileName.CopyTo(stream);
                }
            }


            return "";
        }


        public static string FindFile(string directoryPath, string fileName)
        {
            try
            {
                // Check if the directory exists
                if (Directory.Exists(directoryPath))
                {
                    // Search for files in the directory with a matching base name (excluding extension)
                    var matchingFiles = Directory.GetFiles(directoryPath, fileName + ".*");

                    // Check if any files were found
                    if (matchingFiles.Any())
                    {
                        // Return the first matching file (you can change this logic if you need a different behavior)
                        return matchingFiles[0].Replace("wwwroot", "");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the process
                Console.WriteLine("Error: " + ex.Message);
            }

            // If the file is not found or an error occurred, return null or an appropriate value
            return null;
        }

    }
}

