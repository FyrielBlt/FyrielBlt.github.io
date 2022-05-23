using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BackPfe.Upload
{
    public class UploadFile
    {
        public static string UploadImage(IFormFile imageFile, IWebHostEnvironment _hostEnvironment, string NomDossier)
        {
            FileInfo fi = new FileInfo(imageFile.FileName);
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + fi.Extension;
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, NomDossier, imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }

            return imageName;
        }

        public static void DeleteImage(string imageName, IWebHostEnvironment _hostEnvironment, string NomDossier)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, NomDossier, imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }



    }
}
