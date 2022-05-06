using FileManagerApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;

namespace FileManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : Controller
    {
        // GET: api/files
        // This method returns all files uploaded in Files/Uploads
        [HttpGet]
        public ActionResult Get()
        {
            var list = new List<UploadedFile>();
            var folderName = Path.Combine("Files", "Uploads");
            
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            var files = Directory.GetFiles(folderName);

            foreach (var path in files)
            {
                var size = new FileInfo(path).Length;
                var date = new FileInfo(path).CreationTimeUtc;


                var uploadedFile = new UploadedFile
                {
                    Name = Path.GetFileName(path),
                    Size = size,
                    UploadDate = date
                };

                list.Add(uploadedFile);
            }

            if (list.Count == 0)
                return NotFound();


            return Json(list);
        }

        // POST api/files
        // This method uploads files on filesystem Files/Uploads folder.
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Post([FromForm] IFormFile file)
        {
            try
            {
                IActionResult result = BadRequest();

                var folderName = Path.Combine("Files", "Uploads");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    result = Ok(true);
                }

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
