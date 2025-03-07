﻿using Business.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.FileService
{


    public class FileService
    {
        private readonly IWebHostEnvironment _env;
        private List<string> AllowedExtensions = new List<string>() { ".pdf", ".jepg", ".webp", ".jpg", ".png", ".bmp" };

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string SaveFile(byte[] fileBytes, MediaType folder, string extension)
        {

            if (fileBytes == null)
                return string.Empty;

            if (!AllowedExtensions.Contains(extension))
            {
                return string.Empty;
            }


            string wwwRootPath = _env.WebRootPath;
            string folderPath = Path.Combine(wwwRootPath, folder.ToString());

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fileName = $"{Guid.NewGuid()}{extension}";
            string fullPath = Path.Combine(folderPath, fileName);

            File.WriteAllBytes(fullPath, fileBytes);

            return Path.Combine(folder.ToString(), fileName);
        }

        /*public string SaveFile(RequestAttachment? file, FolderType folder)
        {
            if (file == null)
                return string.Empty;

            return SaveFile(file.File, folder, file.Extension);
        }*/

        public string SaveFile(IFormFile? file, MediaType folder)
        {
            if (file == null)
                return "";

            string extension = Path.GetExtension(file.FileName);
            byte[] fileBytes = ConvertToByteArray(file);

            string wwwRootPath = _env.WebRootPath;
            string folderPath = Path.Combine(wwwRootPath, folder.ToString());

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fileName = $"{Guid.NewGuid()}{extension}";
            string fullPath = Path.Combine(folderPath, fileName);

            File.WriteAllBytes(fullPath, fileBytes);

            return Path.Combine(folder.ToString(), fileName);
        }

        public byte[] ConvertToByteArray(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }


    }
}
